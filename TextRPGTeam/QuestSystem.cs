using System;
using System.Collections.Generic;
using TextRPGTeam;

namespace TextRPGTeam.QuestSystem
{
    // 퀘스트 정보 클래스
    public class Quest
    {
        public int Id { get; }                 // 퀘스트 구분용 ID
        public string Name { get; }            // 퀘스트 제목
        public string Description { get; }     // 퀘스트 설명
        public int RequiredCount { get; }      // 목표 수치
        public int CurrentCount { get; private set; } // 진행도
        public int RewardGold { get; }         // 골드 보상
        public Item RewardItem { get; }      // 보상 아이템

        public bool IsAccepted { get; private set; }
        public bool IsCompleted => CurrentCount >= RequiredCount;
        public bool IsRewardClaimed { get; private set; }
        public void Accept()
        {
            if (!IsAccepted)
            {
                IsAccepted = true;
                Console.WriteLine($"퀘스트 '{Name}' 수락!");
            }
        }

        // 진행도를 올려주는 메서드
        public void IncrementProgress(int amount = 1)
        {
            if (!IsAccepted || IsCompleted) return;

            // 목표치를 넘지 않도록
            CurrentCount = Math.Min(CurrentCount + amount, RequiredCount);
        }

        // 보상 받기 버튼을 눌렀을 때 호출
        public void ClaimReward(Character hero)
        {
            if (!IsCompleted || IsRewardClaimed) return;

            hero.Cash += RewardGold;
            if (RewardItem != null)
                hero.Inventory.Add(RewardItem);

            Console.WriteLine($"보상: {RewardGold}G {(RewardItem != null ? $"+ {RewardItem.Name} 획득" : "")}");
            IsRewardClaimed = true;
        }

        public Quest(int id, string name, string description, int requiredCount, int rewardGold, Item rewardItem = null)
        {
            Id = id;
            Name = name;
            Description = description;
            RequiredCount = requiredCount;
            RewardGold = rewardGold;
            RewardItem = rewardItem;
            CurrentCount = 0;
        }
        public static class QuestDatabase
        {
            // ID로 퀘스트를 생성할 수 있게 함
            private static readonly Dictionary<int, Func<Quest>> _registry = new()
            {
                { 1, () => new Quest(
                        id: 1,
                        name: "미니언 5마리 처치",
                        description: "이봐! 마을 근처에 미니언들이 너무 많아졌다고 생각하지 않나??\n마을주민들의 안전을 위해서라도 저것들 수를 좀 줄여야 한다고!\n자네가 좀 처치해주게!",
                        requiredCount: 5,
                        rewardGold: 5,
                        rewardItem: ""
                    )
                },
                { 2, () => new Quest(
                        id: 2,
                        name: "장비를 장착해보자",
                        description: "장비를 한 번이라도 착용해 보세요!",
                        requiredCount: 1,
                        rewardGold: 10
                    )
                },
                { 3, () => new Quest(
                        id: 3,
                        name: "더 강해지기!",
                        description: "레벨을 2 이상 달성해 보세요!",
                        requiredCount: 2,
                        rewardGold: 20
                    )
                }
            // … 더 많은 퀘스트를 여기서 등록 …
        };


            // 주어진 ID로 새 Quest 인스턴스를 만들어 반환.
            public static Quest Create(int id)
            {
                if (_registry.TryGetValue(id, out var factory))
                    return factory();
                throw new KeyNotFoundException($"Quest ID {id} 가 등록되어 있지 않습니다.");
            }
        }

        // 모든 퀘스트를 관리하는 매니저
        public class QuestManager
        {
            private readonly List<Quest> _quests = new();

            public IReadOnlyList<Quest> AllQuests => _quests;

            //직접 Quest 객체를 추가
            public void AddQuest(Quest quest)
            {
                _quests.Add(quest);
            }

            // ID 하나로 QuestDatabase에서 뽑아와 추가
            public void AddQuestById(int id)
            {
                var q = QuestDatabase.Create(id);
                _quests.Add(q);
            }

            public void OnMonsterDefeated()
            {
                foreach (var q in _quests)
                    if (q.IsAccepted && !q.IsCompleted)
                        q.IncrementProgress();
            }
        }

        // 퀘스트 UI를 담당하는 클래스
        public static class QuestUI
        {
            // 퀘스트 메뉴를 보여주고, 수락·거절·보상받기 처리.
            public static void ShowQuestMenu(QuestManager qm, Character hero)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("\n=== Quest !! ===\n");

                    var quests = qm.AllQuests;

                    for (int i = 0; i < quests.Count; i++)
                    {
                        var q = quests[i];
                        string status = !q.IsAccepted ? "(미수락)"
                                      : q.IsRewardClaimed ? "(보상받음)"
                                      : q.IsCompleted ? "(완료)"
                                      : "(진행중)";
                        Console.WriteLine($"{i + 1}. {q.Name} {status}");
                    }
                    Console.WriteLine("\n0. 나가기");
                    Console.Write("\n원하시는 퀘스트 번호를 입력해주세요!\n>> ");

                    if (!int.TryParse(Console.ReadLine(), out int sel) || sel < 0 || sel > quests.Count)
                        continue;

                    if (sel == 0)
                    {
                        Console.Clear();
                        break;
                    }

                    var chosen = quests[sel - 1];
                    Console.Clear();
                    Console.WriteLine($"\n--- {chosen.Name} ---\n");
                    Console.WriteLine(chosen.Description);
                    Console.WriteLine($"\n진행도: {chosen.CurrentCount}/{chosen.RequiredCount}");
                    Console.WriteLine($"\n보상: {chosen.RewardGold}G"
                                    + (chosen.RewardItem != null ? $", {chosen.RewardItem}" : ""));

                    if (!chosen.IsAccepted)
                    {
                        Console.WriteLine("\n1. 수락   2. 거절");
                        var input = Console.ReadLine();
                        if (input == "1") chosen.Accept();
                        // 2번(거절)이거나 다른 입력 모두 메뉴로 돌아감
                    }
                    else if (chosen.IsCompleted && !chosen.IsRewardClaimed)
                    {
                        Console.WriteLine("\n1. 보상받기   2. 돌아가기");
                        var input = Console.ReadLine();
                        if (input == "1")
                        {
                            chosen.ClaimReward(hero);
                            Console.WriteLine("엔터키를 누르면 돌아갑니다.");
                            Console.ReadLine();
                        }
                    }
                    else
                    {
                        // 진행중 혹은 이미 보상받음
                        Console.WriteLine("\n엔터키를 누르면 돌아갑니다.");
                        Console.ReadLine();
                    }
                }
            }
        }
    }
}
