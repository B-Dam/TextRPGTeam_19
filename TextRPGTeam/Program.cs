namespace TextRPGTeam
{
    using System.Threading.Tasks.Dataflow;

    namespace TextRPGTeam
    {
        class Item()
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
            public Class(string n, string d, int a, int de, int h = 100) : this()
            {
                Name = n;
                Description = d;
                Att = a;
                Def = de;
                Health = h;
            }
        }
        // 직업

        class Character()
        {
            public int Level = 1;
            public string Name;
            public string Class;
            public float Att;
            public float EqAtt = 0; // 장비 공격력
            public float Def;
            public float EqDef = 0; // 장비 방어력
            public int Health;
            public int Cash = 1500;
        }
        // 플레이어

        static class Constants
        {
            public const float sale = 0.85f; // 아이템 판매시 배율
        }

        internal class Program
        {
            static void Main(string[] args)
            {
                Character hero = new Character(); // 플레이어 정보
                
                Class[] job = // 직업
                    [
                         new Class("전사", "전사입니다.", 10, 5),
                         new Class("도적", "도적입니다.", 15, 3)
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
                    Console.Write("1. 상태 보기\n\n2. 인벤토리\n\n3. 상점\n\n4. 던전입장\n\n5. 휴식하기\n\n>>");

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
                                Inven(inventory, hero);//인벤보기
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                                Store(shop, inventory, hero);// 상점가기
                                break;
                            }
                        case 4:
                            {
                                Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                                break;
                            }
                        case 5:
                            {
                                Console.WriteLine("\n" + choice + "번 선택됨!\n\n");
                                Rest(hero);//회복 하기
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요\n");
                                break;
                            }
                    }
                }
            }
            public static void Status(Character c)
            {
                Console.Clear();

                while (true)
                {
                    Console.WriteLine("\n상태보기\n\n캐릭터의 정보가 표시됩니다.\n\n");
                    Console.WriteLine("Lv. " + string.Format("{0:D2}", c.Level) + "\n");
                    Console.WriteLine(c.Name + " ( " + c.Class + " )\n");
                    Console.WriteLine("공격력 : " + (c.Att + c.EqAtt) + (c.EqAtt == 0 ? "" : " (" + (c.EqAtt > 0 ? "+" : "") + c.EqAtt + ")") + "\n");
                    Console.WriteLine("방어력 : " + (c.Def + c.EqDef) + (c.EqDef == 0 ? "" : " (" + (c.EqDef > 0 ? "+" : "") + c.EqDef + ")") + "\n");
                    Console.WriteLine("체 력 : " + c.Health + "\n");
                    Console.WriteLine("Gold : " + c.Cash + " G\n");
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

            public static void Inven(List<Item> Inventory, Character hero)
            {
                int choice;

                Console.Clear();

                while (true)
                {
                    Console.WriteLine("\n인벤토리\n\n보유 중인 아이템을 관리할 수 있습니다.\n\n\n[아이템 목록]\n");
                    ShowItem(Inventory, true);
                    Console.Write("\n1. 장착 관리\n\n2. 나가기\n\n원하시는 행동을 입력해주세요.\n>>");
                    try { choice = int.Parse(Console.ReadLine()); }
                    catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }
                    switch (choice)
                    {
                        case 1: Console.WriteLine("장착관리를 선택하셨습니다|\n"); Equip(Inventory, hero); break;
                        case 2: Console.WriteLine("나가기를 선택하셨습니다|\n"); break;
                        default: Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); break;
                    }
                    if (choice == 2) { Console.Clear(); break; }
                }
            }
            //인벤토리 보기

            public static void Equip(List<Item> items, Character hero)
            {
                int choice;
                string equipType;

                Console.Clear();

                while (true)
                {
                    Console.WriteLine("\n인벤토리 - 장착 관리\n\n보유 중인 아이템을 관리할 수 있습니다.\n\n\n[아이템 목록]\n");
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
                                }
                            }
                            items[choice - 1].Equip = true;
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
                        Console.Write("[E]");
                    Console.Write(item.Name + "\t| ");
                    if (item.Att != 0)
                        Console.Write("공격력 +" + item.Att + " | ");
                    if (item.Def != 0)
                        Console.Write("방어력 +" + item.Def + " | ");
                    Console.WriteLine(item.Description + "\n");
                }
            }
            // 아이템 리스트 보기

            public static void ShowItem(List<Item> items, List<Item> inven, bool num = false)
            {
                int i = 0;
                foreach (Item item in items)
                {
                    i++;
                    Console.Write("- ");
                    if (num)
                        Console.Write(i + " ");
                    Console.Write(item.Name + "\t| ");
                    if (item.Att != 0)
                        Console.Write("공격력 +" + item.Att + " | ");
                    if (item.Def != 0)
                        Console.Write("방어력 +" + item.Def + " | ");
                    Console.Write(item.Description + " | ");
                    Console.WriteLine(inven.Contains(item) ? "구매완료" : (item.Value + "G"));
                }
            }
            // 아이템 리스트 보기(구매 여부 추가)

            public static void ShowItem(List<Item> items, bool num, bool equip, float sale)
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
                    Console.Write(item.Name + "\t| ");
                    if (item.Att != 0)
                        Console.Write("공격력 +" + item.Att + " | ");
                    if (item.Def != 0)
                        Console.Write("방어력 +" + item.Def + " | ");
                    Console.Write(item.Description + " | ");
                    Console.WriteLine((int)((float)item.Value * sale) + "G");
                }
            }
            // 아이템 리스트 보기(판매용)

            public static void Store(List<Item> Shop, List<Item> Inventory, Character hero)
            {
                int choice;

                Console.Clear();

                while (true)
                {
                    Console.WriteLine("\n상점\n\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
                    Console.WriteLine("[보유 골드]\n\n" + hero.Cash + " G\n\n\n[아이템 목록]\n");
                    ShowItem(Shop, Inventory);
                    Console.WriteLine("\n1. 아이템 구매\n\n2. 아이템 판매\n\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요\n>>");

                    try { choice = int.Parse(Console.ReadLine()); }
                    catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                    switch (choice)
                    {
                        case 0: Console.Clear(); break;
                        case 1: BuyItem(Shop, Inventory, ref hero.Cash); break;
                        case 2: SellItem(Inventory, hero); break;
                        default:
                            Console.Clear();
                            Console.WriteLine("잘못된 입력입니다. 다시 선택해 주세요.\n"); break;
                    }
                    if (choice == 0) break;
                }
            }
            // 상점

            public static void BuyItem(List<Item> Shop, List<Item> Inventory, ref int money)
            {
                int choice;

                Console.Clear();

                while (true)
                {
                    Console.WriteLine("\n상점 - 아이템 구매\n\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
                    Console.WriteLine("[보유 골드]\n\n" + money + " G\n\n\n[아이템 목록]\n");
                    ShowItem(Shop, Inventory, true);
                    Console.WriteLine("\n1. 아이템 구매\n\n0. 나가기");
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
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n");
                    }
                }
            }
            // 아이템 구매

            public static void SellItem(List<Item> Inventory, Character hero)
            {
                int choice;

                Console.Clear();

                while (true)
                {
                    Console.WriteLine("\n상점 - 아이템 판매\n\n필요한 아이템을 얻을 수 있는 상점입니다.\n\n");
                    Console.WriteLine("[보유 골드]\n\n" + hero.Cash + " G\n\n\n[아이템 목록]\n");
                    ShowItem(Inventory, true, true, Constants.sale);
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
                        hero.Cash += (int)((float)Inventory[choice - 1].Value * Constants.sale);
                        hero.EqAtt -= Inventory[choice - 1].Att;
                        hero.EqDef -= Inventory[choice - 1].Def;
                        Inventory.Remove(Inventory[choice - 1]);
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n");
                    }
                }
            }
            // 아이템 판매

            public static void Rest(Character hero)
            {
                int choice;

                Console.Clear();

                while (true)
                {
                    Console.Write("\n휴식하기\n\n500 G 를 내면 체력을 회복할 수 있습니다. ");
                    Console.WriteLine("(보유 골드 : " + hero.Cash + " G | 현재체력 : " + hero.Health + ")\n");
                    Console.WriteLine("\n1. 휴식하기\n\n0. 나가기");
                    Console.Write("\n원하시는 행동을 입력해주세요\n>>");

                    try { choice = int.Parse(Console.ReadLine()); }
                    catch { Console.Clear(); Console.WriteLine("\n잘못된 입력입니다. 다시 선택해 주세요.\n"); continue; }

                    if (choice == 0)
                    {
                        Console.Clear();
                        break;
                    }
                    else if (choice == 1)
                    {
                        if (hero.Health == 100)
                        {
                            Console.Clear();
                            Console.WriteLine("\n이미 체력이 가득 차 있습니다.\n");
                        }
                        else if (hero.Cash < 500)
                        {
                            Console.Clear();
                            Console.WriteLine("\nGold가 부족합니다.\n");
                        }
                        else
                        {
                            hero.Health = 100;
                            hero.Cash -= 500;
                            Console.Clear();
                            Console.WriteLine("\n체력이 회복됩니다...\n");
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
            -------------------------------------------------
            public static void Exp(Character hero, int exp)
            {
            //필요한 경험치를 더한다
            hero.Exp += exp;
            Console.WriteLine($"\n[EXP +{exp}] 현재 경험치: {hero.Exp} / {hero.ExpToLevelUp}");

                //반복
                while (hero.Exp >- hero.ExpToLevelUp) ;
                {

                    hero.Exp -= hero.ExpToLevelUp; //레벨업하면 경험치량 초기화
                    hero.Level ++ ;
                    hero.ExpToLevelUp += 30; //레벨업할수록 필요한 경험치 30씩 증가

            
        
                    //레벨업시
                    hero.Att += 1; //힘 1증가
                    hero.Def += 1; //방어 1증가
                    hero.Health = 100; //체력 100회복
                    hero.Cash += 500; //캐쉬 500원
    
                    Console.WriteLine($"\n레벨업!:{hero.Level}이 되었습니다.");
                    Console.WriteLine("공격력,방어력 증가 Cash 500+");
                }
            }
            
        }
    }
}
