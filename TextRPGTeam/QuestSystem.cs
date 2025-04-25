using System;
using System.Collections.Generic;
using System.Threading;
using TextRPGTeam;

namespace TextRPGTeam.QuestSystem
{
    public enum QuestStatus { InProgress, Completed, Rewarded }

    // 추상 기반 퀘스트 클래스
    public abstract class Quest
    {
        public int Id { get; }
        public string Title { get; }
        public string Description { get; }
        public QuestStatus Status { get; protected set; } = QuestStatus.InProgress; // 퀘스트 진행 상태 (진행중, 완료, 보상받음)
        public Item Reward { get; }
        public bool IsAccepted { get; private set; } = false;

        protected Quest(int id, string title, string description, Item reward)
        {
            Id = id;
            Title = title;
            Description = description;
            Reward = reward;
        }

        // 이벤트 (레벨업, 장착, 몹 처치, 던전 클리어)
        public virtual void OnLevelUp(int newLevel) { }
        public virtual void OnEquipChanged(Item item, bool isEquipped) { }
        public virtual void OnMonsterKilled(string monsterType) { }
        public virtual void OnDungeonCleared(int dungeonLevel) { }

        // 퀘스트 수락 설정
        public void Accept()
        {
            if (Status == QuestStatus.InProgress)
                IsAccepted = true;
        }
        
        // 퀘스트 포기 설정
        public void Abandon()
        {
            if (Status == QuestStatus.InProgress)
                IsAccepted = false;
            ResetProgress();
        }

        // 진행도 초기화 메서드
        protected virtual void ResetProgress() { }

        // 퀘스트 완료 처리
        protected void Complete()
        {
            if (Status == QuestStatus.InProgress)
                Status = QuestStatus.Completed;
        }

        // 보상 수령
        public bool ClaimReward(List<Item> inventory)
        {
            if (Status == QuestStatus.Completed)
            {
                inventory.Add(Reward);
                Status = QuestStatus.Rewarded;
                return true;
            }
            return false;
        }
    }

    // 레벨 달성 퀘스트
    public class LevelQuest : Quest
    {
        public int TargetLevel { get; }

        public LevelQuest(int id, string title, string description, int targetLevel, Item reward)
            : base(id, title, description, reward)
        {
            TargetLevel = targetLevel;
        }

        public override void OnLevelUp(int newLevel)
        {
            if (!IsAccepted || Status != QuestStatus.InProgress)
                return;

            if (newLevel >= TargetLevel) // 레벨이 타겟 레벨 보다 높을 시 완료 처리.
                Complete();
        }
    }

    // 장비 장착 여부 퀘스트
    public class EquipQuest : Quest
    {
        public string RequiredType { get; }

        public EquipQuest(int id, string title, string description,
                          string requiredType, Item reward)
            : base(id, title, description, reward)
        {
            RequiredType = requiredType;
        }

        public override void OnEquipChanged(Item item, bool isEquipped)
        {
            if (!IsAccepted || Status != QuestStatus.InProgress)
                return;

            if (item.Type.Equals(RequiredType, StringComparison.OrdinalIgnoreCase) // 지정된 타입의 장비를 장착 시 완료 처리.
                && isEquipped)
            {
                Complete();
            }
        }
    }

    // 몬스터 처치 퀘스트
    public class KillQuest : Quest
    {
        public string MonsterName { get; }
        public int RequiredCount { get; }
        private int currentCount;
        public int Progress => currentCount;

        public KillQuest(int id, string title, string description,
                         string monsterName, int requiredCount, Item reward)
            : base(id, title, description, reward)
        {
            MonsterName = monsterName;
            RequiredCount = requiredCount;
        }

        public override void OnMonsterKilled(string monsterName)
        {
            if (!IsAccepted || Status != QuestStatus.InProgress)
                return;

            if (Status != QuestStatus.InProgress) return;
            if (monsterName.Equals(MonsterName, StringComparison.OrdinalIgnoreCase)) // 지정된 몬스터 이름과 같다면
            {
                currentCount++;                    // 진행도 증가
                if (currentCount >= RequiredCount) // 진행도를 만족하면, 완료 처리.
                    Complete();
            }
        }
        protected override void ResetProgress()
        {
            currentCount = 0;
        }
    }

    // 던전 클리어 퀘스트
    public class DungeonQuest : Quest
    {
        public int RequiredLevel { get; }

        public DungeonQuest(int id, string title,string description,
                            int requiredLevel,Item reward)
            : base(id, title, description, reward)
        {
            RequiredLevel = requiredLevel;
        }

        public override void OnDungeonCleared(int dungeonLevel)
        {
            if (!IsAccepted || Status != QuestStatus.InProgress)
                return;

            if (dungeonLevel >= RequiredLevel)
                Complete();
        }
    }

    // 퀘스트 매니저
    public class QuestManager
    {
        private readonly List<Quest> quests = new List<Quest>();

        //등록된 모든 퀘스트 읽기 전용 리스트
        public IReadOnlyList<Quest> Quests => quests.AsReadOnly();

        public void AddQuest(Quest quest) => quests.Add(quest);

        // id 로 퀘스트를 수락 처리(없으면 무시)
        public void AcceptQuest(int questId)
        {
            var q = quests.FirstOrDefault(x => x.Id == questId);
            if (q != null)
                q.Accept();
        }

        // 추가: 수락 직후 과거 이벤트 일괄 처리 메서드 오버로딩 // 사실 그냥 AcceptQuest()는 없어져도 될 것 같습니다.
        public void AcceptQuest(int questId, int currentLevel,IEnumerable<Item> equippedItems,int lastDungeonLevel)
        {
            var q = quests.FirstOrDefault(x => x.Id == questId);
            if (q == null) return;
            q.Accept();

            // 이미 발생한 이벤트들 재발생 - 이미 클리어 한 상태로 퀘스트를 받을 경우 확인용.
            OnLevelUp(currentLevel);

            foreach (var it in equippedItems)
                OnEquipChanged(it, true);

            OnDungeonCleared(lastDungeonLevel);
        }

        // id로 퀘스트를 포기 처리
        public void AbandonQuest(int questId)
        {
            var q = quests.FirstOrDefault(x => x.Id == questId);
            q?.Abandon();
        }

        // 레벨업 퀘스트 체크용 메서드
        public void OnLevelUp(int newLevel)
        {
            foreach (var q in quests)
                q.OnLevelUp(newLevel);
        }

        // 장비 장착 퀘스트 체크용 메서드
        public void OnEquipChanged(Item item, bool isEquipped)
        {
            foreach (var q in quests)
                q.OnEquipChanged(item, isEquipped);
        }

        // 몬스터 처치 퀘스트 체크용 메서드
        public void OnMonsterKilled(string monsterName)
        {
            foreach (var q in quests)
                q.OnMonsterKilled(monsterName);
        }

        // 던전 클리어 체크용 메서드
        public void OnDungeonCleared(int dungeonLevel)
        {
            foreach (var q in quests)
                q.OnDungeonCleared(dungeonLevel);
        }

        // 완료된 퀘스트에 대해 보상 지급
        public void CheckRewards(List<Item> inventory)
        {
            foreach (var q in quests)
            {
                if (q.Status == QuestStatus.Completed)
                    if (q.ClaimReward(inventory))
                        Console.WriteLine($"퀘스트 '{q.Title}' 완료! 보상 '{q.Reward.Name}' 획득!");
            }
        }

        // 퀘스트 상태 출력, 아직 사용하는 곳 없음
        public void ShowQuestStatus()
        {
            Console.WriteLine("\n< 퀘스트 현황 >");
            foreach (var q in quests)
            {
                Console.WriteLine($"[{q.Status}] {q.Title}: {q.Description}");
                if (q is KillQuest kq)
                    Console.WriteLine($" - 진행도: {kq.Progress}/{kq.RequiredCount}");
            }
        }
    }
}

//레벨업 후
//questMgr.OnLevelUp(hero.Level);

////장비 장착/ 해제시
//questMgr.OnEquipChanged(selectedItem, isNowEquipped);

////몬스터 처치 직후 
//questMgr.OnMonsterKilled(monster.Type);

////던전 클리어시
//questMgr.OnDungeonCleared(currentDungeonLevel);

////퀘스트 진행도 확인 UI
//questMgr.ShowQuestStatus();