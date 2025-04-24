using global::TextRPGTeam.QuestSystem;
using System;
using System.Threading.Tasks.Dataflow;
using TextRPGTeam.QuestSystem; // using문 추가

namespace TextRPGTeam
{
    public class Item() // 아이템 클래스 퍼블릭으로 변경!
    {
        public string Name;
        public string Description;
        public int Att;
        public int Def;
        public bool Equip;
        public int Value;
        public string Type; // 착용 부위

        public Item(string name, string description, int att, int def, int value, string type, bool equip = false) : this()
        {
            Name = name;
            Description = description;
            Att = att;
            Def = def;
            Value = value;
            Type = type;
            Equip = equip;
        }
    }
    // 아이템

    struct Class()
    {
        public string Name;
        public string Description;
        public int Att;
        public int Def;
        public int Health;
        public int Mana;
        public Class(string n, string d, int a, int de, int h = 100, int m = 50) : this()
        {
            Name = n;
            Description = d;
            Att = a;
            Def = de;
            Health = h;
            Mana = m;
        }
    }
    // 직업

    public class Character()
    {
        public int Level = 1;
        public string Name;
        public string Class;
        public float Att;
        public float EqAtt = 0; // 장비 공격력
        public float Def;
        public float EqDef = 0; // 장비 방어력
        public int Health;
        public int Mana;
        public int MaxHealth;
        public int MaxMana;
        public int Cash = 1500;
        public int Exp = 0; //경험치
        public int ExpToLevelUp = 30;//필요경험치
        public Random random = new Random();
        public int CritRate = 15; // 치명타 확률
        public float CritMultiplier = 1.6f; // 치명타 배율
    }
    // 플레이어

    struct Potion()
    {
        public string Name;
        public string Description;
        public int Heal;
        public int Mana;
        public int Value;
        public Potion(string n, string d, int h, int m, int v) : this()
        {
            Name = n;
            Description = d;
            Heal = h;
            Mana = m;
            Value = v;
        }
    }

    class PotionInven()
    {
        public Potion potion;
        public int Count;
        public PotionInven(Potion P, int c) : this()
        {
            potion = P;
            Count = c;
        }
    }

    struct Monster()
    {
        public int Level;
        public string Name;
        public int Hp;
        public int Att;
        public int EvadeRate = 10; // 회피
        public Monster(int l, string n, int h, int a) : this()
        {
            Level = l;
            Name = n;
            Hp = h;
            Att = a;
        }
    }

    static class Constants
    {
        public const float sale = 0.85f; // 아이템 판매시 배율
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("::::::::::: :::::::::: :::    ::: :::::::::::      :::::::::  :::::::::   ::::::::  ");
            Console.WriteLine("    :+:     :+:        :+:    :+:     :+:          :+:    :+: :+:    :+: :+:    :+: ");
            Console.WriteLine("    +:+     +:+         +:+  +:+      +:+          +:+    +:+ +:+    +:+ +:+        ");
            Console.WriteLine("    +#+     +#++:++#     +#++:+       +#+          +#++:++#:  +#++:++#+  :#:        ");
            Console.WriteLine("    +#+     +#+         +#+  +#+      +#+          +#+    +#+ +#+        +#+   +#+# ");
            Console.WriteLine("    #+#     #+#        #+#    #+#     #+#          #+#    #+# #+#        #+#     +# ");
            Console.WriteLine("    ###     ########## ###    ###     ###          ###    ### ###         ########  ");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("-시작하시려면 아무 키나 입력해주세요.");
            Console.ReadKey();
            Console.ResetColor();
            Console.Clear();

            Character hero = new Character(); // 플레이어 정보

            Class[] job = // 직업
                [
                     new Class("전사", "전사입니다.", 10, 5,100,50),
                         new Class("도적", "도적입니다.", 15, 3, 80, 50),
                         new Class("마법사", "마법사입니다.", 8, 6, 70, 100)
                ];

            List<Item> shop = new List<Item> // 상점 아이템
                {
                new Item("수련자갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 5, 1000,"chest"),
                new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 9, 2000,"chest"),
                new Item("스파르타의 갑옷", "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 0, 15, 3500,"chest"),
                new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 2, 0, 600,"weapon"),
                new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 5, 0, 1500,"weapon"),
                new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 7, 0, 3000,"weapon")
                };

            List<Item> inventory = new List<Item> // 인벤토리 아이템
                {
                new Item("수류연타", "물의 태세가 극에 달하여 물 흐르듯 3회의 연격을 날린다.", 5, 20, 1500,"weapon"),
                new Item("암흑강타", "악의 태세가 극에 달하여 강렬한 일격을 날린다.", 25, 5, 1500,"weapon")
                };

            Potion redPotion = new Potion("빨강포션", "체력 30 회복", 30, 0, 100);
            Potion bluePotion = new Potion("파랑포션", "마나 50 회복", 0, 50, 70);
            Potion highPotion = new Potion("엘릭서", "체력&마나 100 회복", 100, 100, 1000);

            PotionInven[] potionInventory = { new PotionInven(redPotion, 3), new PotionInven(bluePotion, 0), new PotionInven(highPotion, 1) };

            List<Monster> mob = new List<Monster> {
                    new Monster(2,"미니언",15,5),
                    new Monster(3,"공허충",10,9),
                    new Monster(5,"대포미니언",25,8)
                };

            var questMgr = new QuestManager(); // 퀘스트 매니저 추가

            // 퀘스트 추가는 여기서
            questMgr.AddQuest(new EquipQuest(
             id: 1,
             title: "무기 장착",
             description: "아무 무기나 장착하세요.",
             requiredType: "weapon",
             reward: new Item("보상 아이템 이름", "보상 아이템 설명.", 0, 0, 100, "아이템 타입")
            ));

            questMgr.AddQuest(new KillQuest(
                id: 2,
                title: "미니언 2마리 처치",
                description: "던전에서 미니언을 2마리 처치하세요.",
                monsterName: "미니언",
                requiredCount: 2,
                reward: new Item("아이템 이름", "아이템 설명.", 0, 0, 0, "아이템 타입")
            ));

            questMgr.AddQuest(new LevelQuest(
                id: 3,
                title: "레벨 업! 2 레벨!",
                description: "2레벨을 달성해보세요!",
                targetLevel: 2,
                reward: new Item("짱 좋은 아이템", "짱 좋은 아이템이에요!", 0, 0, 10000, "아이템 타입")
            ));

            int choice;
            int count = 0;

            Console.WriteLine("\n어서오세요, 스파르타 던전에!\n\n모험가님의 이름을 알려주세요.\n");

            hero.Name = Console.ReadLine(); // 이름 선택

            Console.Clear();

            while (true) // 직업 선택
            {
                Console.WriteLine("\n어서오세요, " + hero.Name + "님!\n\n모험가님의 직업을 알려주세요.\n\n");

                count = 0;

                foreach (Class c in job)
                {
                    count++;
                    Console.WriteLine(count + ". " + c.Name + " : " + c.Description + "\n");
                }

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                if (choice > 0 && choice <= count)
                {
                    Console.WriteLine("\n" + job[choice - 1].Name + "를 선택하셨습니다!\n");
                    hero.Class = job[choice - 1].Name;
                    hero.Att = job[choice - 1].Att;
                    hero.Def = job[choice - 1].Def;
                    hero.Health = job[choice - 1].Health;
                    hero.Mana = job[choice - 1].Mana;
                    hero.MaxHealth = job[choice - 1].Health;
                    hero.MaxMana = job[choice - 1].Mana;
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n");
                }
            }

            Console.Clear();

            while (true) // 메인 화면
            {
                Console.WriteLine("\n" + hero.Name + "님, 다음은 무엇을 할지 선택해 주세요.\n\n");
                Console.Write("1. 상태 보기\n2. 인벤토리\n3. 상점\n4. 던전입장\n5. 회복\n6. 퀘스트\n\n\n0. 캐릭터 직업 변경\n\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                switch (choice)
                {
                    case 1:
                        {
                            Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                            Status(hero);//상태보기
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                            Inven(inventory, hero, questMgr);//인벤보기
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                            Store(shop, inventory, hero,potionInventory);// 상점가기
                            break;
                        }
                    case 4:
                        {
                            Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                            Battle(mob, hero, questMgr);
                            break;
                        }
                    case 5:
                        {
                            Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                            Rest(hero, potionInventory);//회복 하기
                            break;
                        }
                    case 6:
                        {
                            Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                            ShowQuest(questMgr, inventory);
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요\n");
                            break;
                        }
                }
            }
        }
        public static void Status(Character c)
        {
            Console.Clear();

            static void PrintColor(string message, ConsoleColor color)
            {
                Console.ForegroundColor = color; // 컬러 지정
                Console.WriteLine(message);
                Console.ResetColor(); // 컬러 초기화
            }

            while (true)
            {
                PrintColor("\n<상태보기>", ConsoleColor.Cyan);
                Console.WriteLine("\n캐릭터의 정보가 표시됩니다.\n\n");
                Console.Write("Lv. ");
                PrintColor($"{c.Level:D2}", ConsoleColor.Blue);
                Console.WriteLine(c.Name + " ( " + c.Class + " )\n");
                Console.WriteLine("공격력 : " + (c.Att + c.EqAtt) + (c.EqAtt == 0 ? "" : " (" + (c.EqAtt > 0 ? "+" : "") + c.EqAtt + ")") + "\n");
                Console.WriteLine("방어력 : " + (c.Def + c.EqDef) + (c.EqDef == 0 ? "" : " (" + (c.EqDef > 0 ? "+" : "") + c.EqDef + ")") + "\n");
                Console.WriteLine("체 력 : " + c.Health + " / " + c.MaxHealth + "\n");
                Console.WriteLine("마 력 : " + c.Mana + " / " + c.MaxMana + "\n");
                Console.WriteLine("Gold : " + c.Cash + " G\n");
                Console.WriteLine($"현재 경험치: {c.Exp} / {c.ExpToLevelUp}");
                Console.Write("\n\n0. 나가기\n\n원하시는 행동을 입력해주세요.\n>>");
                if (Console.ReadLine() != "0")
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요\n");
                }
                else
                {
                    Console.Clear();
                    break;
                }
            }
        }
        // 캐릭터 정보 보기

        public static void Inven(List<Item> Inventory, Character hero, QuestManager questMgr)
        {
            int choice;

            Console.Clear();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n<인벤토리>");
                Console.ResetColor();
                Console.WriteLine("\n보유 중인 아이템을 관리할 수 있습니다.\n\n\n[아이템 목록]\n");
                ShowItem(Inventory, true);
                Console.Write("\n1. 장착 관리\n\n2. 나가기\n\n원하시는 행동을 입력해주세요.\n>>");
                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }
                switch (choice)
                {
                    case 1: Console.WriteLine("장착관리를 선택하셨습니다|\n"); Equip(Inventory, hero, questMgr); break;
                    case 2: Console.WriteLine("나가기를 선택하셨습니다|\n"); break;
                    default: Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); break;
                }
                if (choice == 2) { Console.Clear(); break; }
            }
        }
        //인벤토리 보기

        public static void Equip(List<Item> items, Character hero, QuestManager questMgr)
        {
            int choice;
            string equipType;

            Console.Clear();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n<인벤토리 - 장착 관리>");
                Console.ResetColor();
                Console.WriteLine("\n보유 중인 아이템을 관리할 수 있습니다.\n\n\n[아이템 목록]\n");
                ShowItem(items, true, true);
                Console.WriteLine("\n0. 나가기");
                Console.Write("\n\n원하시는 행동을 입력해주세요.\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                if (choice == 0)
                {
                    Console.Clear();
                    break;
                }
                else if (choice > 0 && choice <= items.Count)
                {
                    equipType = items[choice - 1].Type;
                    if (items[choice - 1].Equip)//선택한 장비가 장착중이면 해제
                    {
                        items[choice - 1].Equip = false;
                        hero.EqAtt -= items[choice - 1].Att;
                        hero.EqDef -= items[choice - 1].Def;
                        questMgr.OnEquipChanged(items[choice - 1], false);
                    }
                    else
                    {
                        for (int i = 0; i < items.Count; i++)//선택한 장비와 같은 타입의 장비착용시 해제
                        {
                            if (equipType == items[i].Type && items[i].Equip == true)
                            {
                                items[i].Equip = false;
                                hero.EqAtt -= items[i].Att;
                                hero.EqDef -= items[i].Def;
                                questMgr.OnEquipChanged(items[i], false);
                            }
                        }
                        items[choice - 1].Equip = true;
                        questMgr.OnEquipChanged(items[choice - 1], true);
                        hero.EqAtt += items[choice - 1].Att;
                        hero.EqDef += items[choice - 1].Def;
                    }
                    Console.Clear();
                }
                else
                { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); }
            }
        }
        // 장착관리

        public static void ShowItem(List<Item> items, bool equip, bool num = false)
        {
            int i = 0;

            foreach (Item item in items)
            {
                i++;
                Console.Write("- ");
                if (num)
                    Console.Write(i + " ");
                if (item.Equip && equip)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen; // 아이템 장착시 컬러 변경
                    Console.Write("[E]");
                }
                Console.Write($"{PadRightForConsole(item.Name,16)}| ");
                if (item.Att != 0)
                    Console.Write("공격력 +" + item.Att + " | ");
                if (item.Def != 0)
                    Console.Write("방어력 +" + item.Def + " | ");
                Console.WriteLine(item.Description + "\n");
                Console.ResetColor(); // 여기까지 컬러 변경 영역
            }
        }
        // 아이템 리스트 보기

        public static int ShowItem(List<Item> items, List<Item> inven, bool num = false)
        {
            int i = 0;
            foreach (Item item in items)
            {
                i++;
                Console.Write("- ");
                if (num)
                    Console.Write(i + " ");
                Console.Write($"{PadRightForConsole(item.Name,16)}| ");
                if (item.Att != 0)
                    Console.Write("공격력 +" + item.Att + " | ");
                if (item.Def != 0)
                    Console.Write("방어력 +" + item.Def + " | ");
                Console.Write(item.Description + " | ");
                if (inven.Contains(item))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("구매완료");
                }
                else
                {
                    Console.WriteLine(item.Value + "G");
                }

                Console.ResetColor();
            }
            return i;
        }
        // 아이템 리스트 보기(구매 여부 추가)

        public static int ShowItem(List<Item> items, bool num, bool equip, float sale)
        {
            int i = 0;

            foreach (Item item in items)
            {
                i++;
                Console.Write("- ");
                if (num)
                    Console.Write(i + " ");
                if (item.Equip && equip)
                    Console.Write("[E]");
                Console.Write($"{PadRightForConsole(item.Name,16)}| ");
                if (item.Att != 0)
                    Console.Write("공격력 +" + item.Att + " | ");
                if (item.Def != 0)
                    Console.Write("방어력 +" + item.Def + " | ");
                Console.Write(item.Description + " | ");
                Console.WriteLine((int)((float)item.Value * sale) + "G");
            }
            return i;
        }
        // 아이템 리스트 보기(판매용)
        public static void Store(List<Item> Shop, List<Item> Inventory, Character hero, PotionInven[] potion)
        {
            int choice;

            Console.Clear();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n<상점>");
                Console.ResetColor();
                Console.WriteLine("\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
                Console.WriteLine("[보유 골드]\n\n" + hero.Cash + " G\n\n\n[아이템 목록]\n");
                ShowItem(Shop, Inventory);
                foreach (PotionInven pot in potion)
                {
                    Console.WriteLine($"- {PadRightForConsole(pot.potion.Name,16)}| {pot.potion.Description} | {pot.potion.Value}");
                }
                Console.WriteLine("\n1. 아이템 구매\n\n2. 아이템 판매\n\n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                switch (choice)
                {
                    case 0: Console.Clear(); break;
                    case 1: BuyItem(Shop, Inventory, ref hero.Cash,potion); break;
                    case 2: SellItem(Inventory, hero,potion); break;
                    default:
                        Console.Clear();
                        Console.WriteLine("잘못된 입력입니다. 다시 선택해 주세요.\n"); break;
                }
                if (choice == 0) break;
            }
        }
        // 상점

        public static void BuyItem(List<Item> Shop, List<Item> Inventory, ref int money, PotionInven[] potion)
        {
            int choice;
            int maxNumber;
            Console.Clear();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n<상점 - 아이템 구매>");
                Console.ResetColor();
                Console.WriteLine("\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
                Console.WriteLine("[보유 골드]\n\n" + money + " G\n\n\n[아이템 목록]\n");
                maxNumber = ShowItem(Shop, Inventory, true);
                foreach (PotionInven pot in potion)
                {
                    maxNumber++;
                    Console.WriteLine($"- {maxNumber} {PadRightForConsole(pot.potion.Name, 16)}| {pot.potion.Description} | {pot.potion.Value}G ({pot.Count}개 보유)");
                }
                Console.WriteLine("\n\n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                if (choice == 0)
                {
                    Console.Clear();
                    break;
                }
                else if (choice > 0 && choice <= Shop.Count)
                {
                    if (Inventory.Contains(Shop[choice - 1]))
                    {
                        Console.Clear();
                        Console.WriteLine("\n이미 구매한 아이템입니다.\n");
                    }
                    else if (Shop[choice - 1].Value > money)
                    {
                        Console.Clear();
                        Console.WriteLine("\nGold가 부족합니다.\n");
                    }
                    else
                    {
                        Inventory.Add(Shop[choice - 1]);
                        money -= Shop[choice - 1].Value;
                        Console.Clear();
                    }
                }
                else if (choice > Shop.Count && choice <= maxNumber)
                {
                    if (potion[choice - Shop.Count - 1].potion.Value > money)
                    {
                        Console.Clear();
                        Console.WriteLine("\nGold가 부족합니다.\n");
                    }
                    else
                    {
                        potion[choice - Shop.Count - 1].Count++;
                        money -= potion[choice - Shop.Count - 1].potion.Value;
                        Console.Clear();
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n");
                }
            }
        }
        // 아이템 구매

        public static void SellItem(List<Item> Inventory, Character hero, PotionInven[] potion)
        {
            int choice;
            int maxNumber;
            Console.Clear();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n<상점 - 아이템 판매>");
                Console.ResetColor();
                Console.WriteLine("\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
                Console.WriteLine("[보유 골드]\n\n" + hero.Cash + " G\n\n\n[아이템 목록]\n");
                maxNumber=ShowItem(Inventory, true, true, Constants.sale);
                foreach (PotionInven pot in potion)
                {
                    if (pot.Count > 0)
                    {
                        maxNumber++;
                        Console.WriteLine($"- {maxNumber} {PadRightForConsole(pot.potion.Name, 16)}| {pot.potion.Description} | {pot.potion.Value * Constants.sale}G ({pot.Count}개 보유)");
                    }
                }
                Console.WriteLine("\n\n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                if (choice == 0)
                {
                    Console.Clear();
                    break;
                }
                else if (choice > 0 && choice <= Inventory.Count)
                {
                    hero.Cash += (int)(Inventory[choice - 1].Value * Constants.sale);
                    if (Inventory[choice - 1].Equip)
                    {
                        hero.EqAtt -= Inventory[choice - 1].Att;
                        hero.EqDef -= Inventory[choice - 1].Def;
                    }
                    Inventory.Remove(Inventory[choice - 1]);
                    Console.Clear();
                }
                else if (choice > Inventory.Count && choice <= maxNumber)
                {
                    int c = choice - Inventory.Count - 1;
                    foreach (PotionInven pot in potion)
                    {   if(pot.Count <= 0) { continue; }
                        else if (c>0) { c--; continue; }
                        else
                        {
                            hero.Cash += (int)(pot.potion.Value * Constants.sale);
                            pot.Count--;
                            Console.Clear();
                            break;
                        }
                    }

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n");
                }
            }
        }
        // 아이템 판매

        public static void Rest(Character hero, PotionInven[] potionInventory)
        {
            int choice;
            int count;
            Console.Clear();

            while (true)
            {
                count = 0;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\n<회복>\n");
                Console.ResetColor();
                Console.WriteLine("\n포션을 사용하여 회복할 수 있습니다. \n\n");
                Console.WriteLine($"(현재체력 : {hero.Health}/{hero.MaxHealth} / 현재마나 : {hero.Mana}/{hero.MaxMana})\n\n");
                foreach (PotionInven potion in potionInventory)
                {
                    count++;
                    Console.WriteLine($"{count}. {potion.potion.Name} : {potion.potion.Description} ({potion.Count}개)\n");
                }
                Console.WriteLine("\n0. 나가기");
                Console.Write("\n원하시는 행동을 입력해주세요\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                if (choice == 0)
                {
                    Console.Clear();
                    break;
                }
                else if (choice > 0 && choice <= count)
                {
                    if (0 >= potionInventory[choice - 1].Count)
                    {
                        Console.Clear();
                        Console.WriteLine($"\n{potionInventory[choice - 1].potion.Name}(이)가 부족합니다.\n");
                    }
                    else
                    {
                        potionInventory[choice - 1].Count--;
                        hero.Health += potionInventory[choice - 1].potion.Heal;
                        if (hero.Health > hero.MaxHealth) hero.Health = hero.MaxHealth;
                        hero.Mana += potionInventory[choice - 1].potion.Mana;
                        if (hero.Mana > hero.MaxMana) hero.Mana = hero.MaxMana;
                        Console.Clear();
                        Console.WriteLine($"\n{potionInventory[choice - 1].potion.Name}을(를) 사용하였습니다!\n");
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n");
                }
            }
        }
        //휴식

        public static void Battle(List<Monster> mob, Character hero, QuestManager questMgr)//배틀 메소드
        {
            bool allDead;
            Random random = new Random();
            int mobCount = random.Next(1, 4);//몬스터 생성 마릿수
            int[] enemyHealth = new int[mobCount];//몬스터 체력 저장 변수, class는 같은 종류의 몬스터들의 체력을 하나로 보아 필요
            int i;
            List<Monster> enemy = new List<Monster> { };//전투시의 적 리스트

            for (i = 0; i < mobCount; i++)
            {
                enemy.Add(mob[random.Next(0, mob.Count)]);//mob리스트 안의 몬스터를 랜덤으로 enemy에 추가
                enemyHealth[i] = enemy[i].Hp;
            }
            int choice;


            while (true)
            {
                Console.Clear();
                if (hero.Health <= 0)
                {
                    Console.WriteLine("\n현재 체력이 없습니다. 마을로 돌아갑니다...\n");
                    break;
                }
                allDead = true;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nBattle!!\n\n");
                Console.ResetColor();
                i = 0;
                foreach (Monster enm in enemy)
                {
                    if (enemyHealth[i] > 0)
                    {
                        Console.WriteLine($"  Lv.{enm.Level}  {PadRightForConsole(enm.Name, 15)}HP {enemyHealth[i]}");
                        allDead = false;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"  Lv.{enm.Level}  {PadRightForConsole(enm.Name, 15)}Dead");
                        Console.ResetColor();
                    }
                    i++;
                }
                if (allDead) { Console.Clear(); BattleVictory(enemy, hero, questMgr); Console.Clear(); break; }

                Console.Write($"\n\n\n[내정보]\n\nLv.{hero.Level} {hero.Name} \t ({hero.Class})\n\nHP {hero.Health}/{hero.MaxHealth}\n\nMP {hero.Mana}/{hero.MaxMana}\n\n");
                Console.Write("\n1. 공격\n\n원하시는 행동을 입력해주세요.\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                switch (choice)
                {
                    case 1: BattleAttack(enemy, hero, enemyHealth, questMgr); break;
                    default: Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); break;
                }
            }
        }
        public static void BattleAttack(List<Monster> enemy, Character hero, int[] enemyHealth, QuestManager questMgr) //플레이어 공격시 메소드
        {
            Random random = new Random();
            int choice;
            int count;
            Monster foe;
            int damage;
            Console.Clear();
            while (true)
            {
                count = 0;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nBattle!!\n\n");
                Console.ResetColor();
                foreach (Monster enm in enemy)
                {
                    count++;
                    if (enemyHealth[count - 1] > 0)
                        Console.WriteLine($"{count} Lv.{enm.Level}  {PadRightForConsole(enm.Name, 15)}HP {enemyHealth[count - 1]}");
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"  Lv.{enm.Level}  {PadRightForConsole(enm.Name, 15)}Dead");
                        Console.ResetColor();
                    }
                }

                Console.Write($"\n\n\n[내정보]\n\nLv.{hero.Level} {hero.Name} \t ({hero.Class})\n\nHP {hero.Health}/{hero.MaxHealth}\n\nMP {hero.Mana}/{hero.MaxMana}\n\n");
                Console.Write("\n0. 취소\n\n대상을 선택해주세요.\n>>");

                try { choice = int.Parse(Console.ReadLine()); }
                catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }
                if(choice == 0) { Console.Clear(); break; }
                else if (choice > 0 && choice <= count && enemyHealth[choice - 1] > 0)
                {
                    Console.Clear();

                    int targetIndex = choice - 1;
                    foe = enemy[targetIndex];

                    bool isEvaded = random.Next(0, 100) < foe.EvadeRate;
                    bool isCritical = random.Next(0, 100) < hero.CritRate;
                    damage = (int)(hero.Att + hero.EqAtt) + random.Next(-1, 2);//공격력과 장비공격력을 더하고 오차 +-1의 데미지

                    if (isEvaded) //몬스터 회피
                    {
                        damage = 0;
                        Console.Clear();
                        Console.Write($"\nBattle!!\n\n\n{hero.Name}의 공격!\n\n");
                        Console.WriteLine($"{foe.Name} 을(를) 공격했지만 아무일도 일어나지 않았습니다.");
                        Console.Write("\n\n\n아무버튼이나 누르세요..");
                        Console.ReadLine();
                        EnemyAttack(enemy, hero, enemyHealth, questMgr);
                        break;
                    }
                    else
                    {
                        string critText = "";
                        if (isCritical) //플레이어 치명타
                        {
                            damage = (int)(damage * hero.CritMultiplier);
                            critText = " - 치명타 공격!!";
                        }

                        enemyHealth[targetIndex] -= damage;
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("\nBattle!!\n\n\n");
                        Console.ResetColor();
                        Console.Write($"{hero.Name}의 공격!\n\n");
                        Console.Write($"Lv.{foe.Level} {foe.Name} 을(를) 맞췄습니다.");
                        Console.Write($"[데미지 : {damage}]{critText}\n\n\n");
                        Console.Write($"Lv.{foe.Level} {foe.Name}\n\n");
                        if (enemyHealth[targetIndex] > 0)
                            Console.Write($"HP {enemyHealth[targetIndex] + damage} -> {enemyHealth[targetIndex]}");
                        else
                        {
                            Console.Write($"HP {enemyHealth[targetIndex] + damage} -> Dead");
                            questMgr.OnMonsterKilled(foe.Name);
                        }
                        Console.Write("\n\n\n아무버튼이나 누르세요..");
                        Console.ReadLine();

                        EnemyAttack(enemy, hero, enemyHealth, questMgr);
                        break;
                    }
                }
                else { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); }
            }
        }
        public static void EnemyAttack(List<Monster> enemy, Character hero, int[] enemyHealth, QuestManager questMgr)//적군 공격시 메소드
        {
            Random random = new Random();
            int damage;
            int i = -1;
            foreach (Monster enm in enemy)
            {
                i++;
                if (enemyHealth[i] <= 0) continue;
                Console.Clear();
                damage = enm.Att + random.Next(-1, 2);
                hero.Health -= damage;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("\nBattle!!\n\n\n");
                Console.ResetColor();
                Console.Write($"Lv.{enm.Level} {enm.Name} 의 공격!\n\n");
                Console.Write($"{hero.Name} 을(를) 맞췄습니다. [데미지 : {damage}]\n\n\n");
                Console.Write($"Lv.{hero.Level} {hero.Name}\n\n");
                if (hero.Health > 0)
                    Console.Write($"HP {hero.Health + damage} -> {hero.Health}\n\n\n");
                else
                    Console.Write($"HP {hero.Health + damage} -> Dead\n\n\n");
                Console.Write("아무버튼이나 누르세요..");
                Console.ReadLine();
                if (hero.Health <= 0)
                {
                    BattleDefeat(enemy, hero);
                    break;
                }
            }
        }
        public static void BattleVictory(List<Monster> enemy, Character hero, QuestManager questMgr) //배틀 승리시 메소드
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nBattle - Result\n\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Victory\n\n");
            Console.ResetColor();
            Console.WriteLine($"던전에서 몬스터 {enemy.Count}마리를 잡았습니다.\n\n");

            int totalExp = enemy.Count * 10; //몬스터 x 경험치10

            Console.WriteLine($"Lv.{hero.Level} {hero.Name}\n");
            Console.WriteLine($"HP {hero.Health}/100\n\n");
            Console.WriteLine($"경험치를 흭득하셨습니다:{totalExp}"); //승리시 경험치 흭득 
            Exp(hero, totalExp, questMgr);
            Console.Write("아무버튼이나 누르세요..");
            Console.ReadLine();
        }
        public static void BattleDefeat(List<Monster> enemy, Character hero) //배틀 패배시 메소드
        {
            hero.Health = 0;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nBattle - Result\n\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("You Lose\n\n");
            Console.ResetColor();
            Console.WriteLine($"Lv.{hero.Level} {hero.Name}\n");
            Console.WriteLine($"HP {hero.Health}/100\n\n");
            Console.Write("아무버튼이나 누르세요..");
            Console.ReadLine();
        }
        public static string PadRightForConsole(string input, int totalWidth)//글자 여백 메소드, 한글은 출력 너비가 영어와 달라 사용
        {
            int visualLength = 0;
            foreach (char c in input)
                visualLength += (c >= 0xAC00 && c <= 0xD7A3) ? 2 : 1; // 한글은 2, 나머지는 1

            int padding = Math.Max(0, totalWidth - visualLength);
            return input + new string(' ', padding);
        }
        public static void Exp(Character hero, int exp, QuestManager questMgr)
        {

            int Level = 1;
            int ExpToLevelUp;

            //필요한 경험치를 더한다
            hero.Exp += exp;
            if (Level == 1)
                ExpToLevelUp = 30;

            //반복
            while (hero.Exp >= hero.ExpToLevelUp)
            {
                hero.Exp -= hero.ExpToLevelUp; //레벨업하면 경험치량 초기화
                hero.Level++;
                questMgr.OnLevelUp(hero.Level); // 레벨 퀘스트 확인용
                hero.ExpToLevelUp += 30; //레벨업할수록 필요한 경험치 30씩 증가

                //레벨업시
                hero.Att += 1; //힘 1증가
                hero.Def += 1; //방어 1증가
                hero.Health = 100; //체력 100회복
                hero.Cash += 500; //캐쉬 500원

                Console.WriteLine($"\n레벨업! {hero.Level}레벨이 되었습니다.");
                Console.WriteLine("공격력이 1 올랐습니다!\n방어력이 1 올랐습니다!\n500 G를 획득했습니다!");
            }
        }

        // 퀘스트 관련 메서드 --------------------------------------------------------------------
        static void ShowQuest(QuestManager qm, List<Item> inventory)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("< 퀘스트 메뉴 >\n");
                Console.ResetColor();
                Console.WriteLine("1. 진행 중 퀘스트");
                Console.WriteLine("2. 수락 가능한 퀘스트");
                Console.WriteLine("\n0. 뒤로");
                Console.Write("\n원하시는 행동을 입력해주세요!\n>> ");

                var choice = Console.ReadLine();
                if (choice == "0")
                {
                    Console.Clear();
                    return;
                }

                if (choice == "1")
                    ShowInProgress(qm, inventory);
                else if (choice == "2")
                    ShowAvailable(qm, inventory);
                else
                {
                    Console.WriteLine("정확히 입력해주세요.\n계속하려면 아무 키나 누르세요.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowInProgress(QuestManager qm, List<Item> inventory)
        {
            static void PrintColor(string message, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }

            var accepted = qm.Quests.Where(q => q.IsAccepted).ToList();

            if (!accepted.Any())
            {
                Console.WriteLine("수락된 퀘스트가 없습니다.\n계속하려면 아무 키나 누르세요.");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                PrintColor("< 진행 중 퀘스트 >\n", ConsoleColor.Cyan);
                Console.WriteLine("이곳에서 수락한 퀘스트의 진행도와 상세정보를 확인할 수 있습니다.\n");
                for (int i = 0; i < accepted.Count; i++)
                {
                    var q = accepted[i];

                    var statusKr = q.Status switch
                    {
                        QuestStatus.InProgress => "진행중",
                        QuestStatus.Completed => "완료",
                        QuestStatus.Rewarded => "보상 받음",
                    };

                    Console.WriteLine($"{i + 1}. [{statusKr}] {accepted[i].Title}");
                }
                Console.Write("\n상세 정보를 확인할 퀘스트의 번호를 입력하세요. (0: 돌아가기)\n>> ");

                var input = Console.ReadLine();
                if (input == "0") return;

                if (!int.TryParse(input, out int sel) || sel < 1 || sel > accepted.Count)
                {
                    Console.WriteLine("정확히 입력해주세요.\n계속하려면 아무 키나 누르세요.");
                    Console.ReadKey();
                    continue;
                }

                var quest = accepted[sel - 1];
                Console.Clear();
                var statusKor = quest.Status switch
                {
                    QuestStatus.InProgress => "진행중",
                    QuestStatus.Completed => "완료",
                    QuestStatus.Rewarded => "보상 받음",
                };

                PrintColor("< 퀘스트 상세 >\n", ConsoleColor.Cyan);
                PrintColor("퀘스트를 포기하면 진행도가 초기화됩니다! 주의하세요!\n", ConsoleColor.DarkRed);
                Console.WriteLine($"제목: {quest.Title}\n");
                Console.WriteLine($"설명: {quest.Description}\n");
                Console.WriteLine($"상태: [{statusKor}]");
                if (quest is KillQuest kq)
                    Console.WriteLine($"\n진행도: {kq.Progress}/{kq.RequiredCount}");

                if (quest.Status == QuestStatus.Completed)
                {
                    Console.WriteLine("\n1. 보상받기");
                    Console.WriteLine("\n0. 뒤로");
                    Console.Write("\n>> ");
                    var cmd = Console.ReadLine();

                    if (cmd == "1")
                    {
                        if (quest.ClaimReward(inventory))
                            Console.WriteLine($"\n보상 '{quest.Reward.Name}' 을(를) 획득했습니다!");
                        else
                            Console.WriteLine("\n이미 보상을 받았습니다.");
                        Console.WriteLine("계속하려면 아무 키나 누르세요.");
                        Console.ReadKey();
                        return;   // 상세 보기 종료
                    }
                    else if (cmd == "0")
                    {
                        return;
                    }
                }
                else if (quest.Status == QuestStatus.Rewarded)
                {
                    Console.WriteLine("아무키나 누르면 돌아갑니다.");
                    Console.Write($"\n>> ");
                    Console.ReadKey();
                    continue;
                }
                else
                {
                    Console.WriteLine("\n\n1. 퀘스트 포기");
                    Console.WriteLine("\n0. 뒤로");
                    Console.Write("\n>> ");
                    var cmd = Console.ReadLine();
                    if (cmd == "1")
                    {
                        qm.AbandonQuest(quest.Id);
                        Console.WriteLine("퀘스트를 포기했습니다. 수락 가능한 퀘스트로 이동합니다.");
                        Console.ReadKey();
                        ShowAvailable(qm, inventory);
                        return;
                    }
                    else if (cmd != "0")
                    {
                        Console.WriteLine("정확히 입력해주세요.\n계속하려면 아무 키나 누르세요.");
                        Console.ReadKey();
                    }
                }
            }
        }

        static void ShowAvailable(QuestManager qm, List<Item> inv)
        {
            static void PrintColor(string message, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }

            var available = qm.Quests.Where(q => !q.IsAccepted).ToList();
            if (!available.Any())
            {
                Console.WriteLine("수락 가능한 퀘스트가 없습니다.\n계속하려면 아무 키나 누르세요.");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                Console.Clear();
                PrintColor("< 수락 가능한 퀘스트 >\n", ConsoleColor.Cyan);
                for (int i = 0; i < available.Count; i++)
                    Console.WriteLine($"{i + 1}. {available[i].Title}");
                Console.Write("\n상세 정보를 확인할 퀘스트의 번호를 입력하세요. (0: 돌아가기)\n>> ");

                var input = Console.ReadLine();
                if (input == "0") return;

                if (!int.TryParse(input, out int sel) || sel < 1 || sel > available.Count)
                {
                    Console.WriteLine("정확히 입력해주세요.\n계속하려면 아무 키나 누르세요.");
                    Console.ReadKey();
                    continue;
                }

                var quest = available[sel - 1];
                Console.Clear();
                var statusKor = quest.Status switch
                {
                    QuestStatus.InProgress => "진행중",
                    QuestStatus.Completed => "완료",
                    QuestStatus.Rewarded => "보상 받음",
                    _ => ""
                };

                PrintColor("< 퀘스트 상세 >\n", ConsoleColor.Cyan);
                Console.WriteLine($"제목 : {quest.Title}\n");
                Console.WriteLine($"설명 : {quest.Description}\n");
                if (quest is KillQuest kq)
                    Console.WriteLine($"\n진행도 : {kq.Progress}/{kq.RequiredCount}");

                Console.WriteLine("\n\n1. 수락");
                Console.WriteLine("\n0. 돌아가기");
                Console.Write("\n>> ");
                var cmd = Console.ReadLine();
                if (cmd == "1")
                {
                    qm.AcceptQuest(quest.Id);
                    Console.WriteLine($"퀘스트 '{quest.Title}' 수락되었습니다!");
                    Console.ReadKey();
                    return;
                }
                else if (cmd != "0")
                {
                    Console.WriteLine("정확히 입력해주세요.\n계속하려면 아무 키나 누르세요.");
                    Console.ReadKey();
                }
            }
        }
        // 퀘스트 관련 메서드 끝 --------------------------------------------------------------------
    }
}
