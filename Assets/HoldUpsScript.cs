using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;
using System;
using Random = UnityEngine.Random;
using System.Linq;

public class HoldUpsScript : MonoBehaviour
{
    public KMSelectable Move1Button, Move2Button, Move3Button, Move4Button;
    public KMSelectable AllOutAttackBtn, BreakFormationBtn, TalkBtn;
    public KMSelectable MoneyBtn, ItemsBtn;

    public GameObject ThreeDownsObj, FiveDownsObj;
    public GameObject BattlePhase, HoldUpPhase;
    public GameObject HoldUpAnimObject, SolveAnimObject;
    public GameObject AllOutAttackBtnObject, BreakFormationBtnObject, TalkBtnObject, MoneyBtnObject, ItemsBtnObject;
    public GameObject JokerPortrait;
    public GameObject ShadowNameBox;
    public GameObject TakeYourHeart;
    public GameObject ShadowPersonality;

    public KMAudio BombAudio;

    public KMBombModule Module;

    public Renderer Move1Render, Move2Render, Move3Render, Move4Render;
    public Renderer[] MoveTypeRenders;
    public Renderer[] ThreeDownsLights, FiveDownsLights;
    public Renderer HoldUpRender;
    public Renderer ComponentRender;
    public Renderer SolveRender;
    public Renderer AllOutAttackBtnRender, BreakFormationBtnRender, TalkBtnRender;
    public Renderer MoneyBtnRender, ItemsBtnRender;
    public Renderer ShadowPersonalityRender;

    public Texture[] MoveTypeIcons;
    public Texture DefaultMoveIcon;
    public Texture[] HoldUpFrames;
    public Texture[] AllOutAttackFrames, FlagFrames, TransitFrames;
    public Texture[] AllOutAttackBtnStates, BreakFormationBtnStates, TalkBtnStates;
    public Texture[] MoneyBtnStates, ItemsBtnStates;
    public Texture[] AllShadowPersonalities;

    public Material RedComponent;

    public TextMesh[] MoveTexts;
    public TextMesh ShadowName;
    public TextMesh DownedShadowText;

    public KMBombInfo BombInfo;

    List<string> ShadowsWeakToFire = new List<string>()
    {
        "Mandrake", "Silky", "Koropokguru", "Nue", "Jack Frost", "Leanan Sidhe"
    };
    List<string> ShadowsWeakToIce = new List<string>()
    {
        "Hua Po", "Orthrus", "Lamia"
    };
    List<string> ShadowsWeakToElec = new List<string>()
    {
        "Bicorn", "Kelpie", "Apsaras", "Makami", "Nekomata", "Sandman", "Naga"
    };
    List<string> ShadowsWeakToWind = new List<string>()
    {
        "Agathion", "Berith", "Mokoi", "Inugami"
    };
    List<string> ShadowsWeakToPsy = new List<string>()
    {
        "Take-Minakata", "Thoth", "Isis"
    };
    List<string> ShadowsWeakToNuclear = new List<string>()
    {
        "High Pixie", "Yaksini", "Anzu"
    };
    List<string> ShadowsWeakToGun = new List<string>()
    {
        "Jack O’ Lantern", "Succubus", "Andras"
    };
    List<string> ShadowsWeakToBless = new List<string>()
    {
        "Incubis", "Onmoraki", "Koppa-Tengu", "Orobas", "Rakshasa"
    };
    List<string> ShadowsWeakToCurse = new List<string>()
    {
        "Pixie", "Angel"
    };

    public List<string> AllDownedShadows;

    public string[] MoveTypes;
    public string[] Moves;
    public string[] AllWeaknesses;
    public string Shadow, Weakness;
    public string DesiredHoldUpAction;
    string SelectedDownedShadow;
    public string Personality;

    bool FiveDowns;

    int CurrentDown;
    int CurrentShadow;
    int StageNr = 0;

    static int moduleIdCounter = 1;
    int moduleID;

    void Awake()
    {
        Move1Button.OnInteract = Move1;
        Move2Button.OnInteract = Move2;
        Move3Button.OnInteract = Move3;
        Move4Button.OnInteract = Move4;

        AllOutAttackBtn.OnInteract = HandleAllOutAttack;
        BreakFormationBtn.OnInteract = HandleBreak;
        TalkBtn.OnInteract = HandleNegotiation;

        MoneyBtn.OnInteract = HandleMoney;
        ItemsBtn.OnInteract = HandleItems;

        Move1Button.OnHighlight = delegate
        {
            Move1Render.material.color = new Color32(253, 0, 35, 255);
        };
        Move1Button.OnHighlightEnded = delegate
        {
            Move1Render.material.color = new Color32(124, 124, 124, 255);
        };

        Move2Button.OnHighlight = delegate
        {
            Move2Render.material.color = new Color32(253, 0, 35, 255);
        };
        Move2Button.OnHighlightEnded = delegate
        {
            Move2Render.material.color = new Color32(124, 124, 124, 255);
        };

        Move3Button.OnHighlight = delegate
        {
            Move3Render.material.color = new Color32(253, 0, 35, 255);
        };
        Move3Button.OnHighlightEnded = delegate
        {
            Move3Render.material.color = new Color32(124, 124, 124, 255);
        };

        Move4Button.OnHighlight = delegate
        {
            Move4Render.material.color = new Color32(253, 0, 35, 255);
        };
        Move4Button.OnHighlightEnded = delegate
        {
            Move4Render.material.color = new Color32(124, 124, 124, 255);
        };
        
        AllOutAttackBtn.OnHighlight = delegate
        {
            AllOutAttackBtnRender.material.mainTexture = AllOutAttackBtnStates[1];
        };
        AllOutAttackBtn.OnHighlightEnded = delegate
        {
            AllOutAttackBtnRender.material.mainTexture = AllOutAttackBtnStates[0];
        };

        BreakFormationBtn.OnHighlight = delegate
        {
            BreakFormationBtnRender.material.mainTexture = BreakFormationBtnStates[1];
        };
        BreakFormationBtn.OnHighlightEnded = delegate
        {
            BreakFormationBtnRender.material.mainTexture = BreakFormationBtnStates[0];
        };

        TalkBtn.OnHighlight = delegate
        {
            TalkBtnRender.material.mainTexture = TalkBtnStates[1];
        };
        TalkBtn.OnHighlightEnded = delegate
        {
            TalkBtnRender.material.mainTexture = TalkBtnStates[0];
        };

        MoneyBtn.OnHighlight = delegate
        {
            MoneyBtnRender.material.mainTexture = MoneyBtnStates[1];
        };
        MoneyBtn.OnHighlightEnded = delegate
        {
            MoneyBtnRender.material.mainTexture = MoneyBtnStates[0];
        };

        ItemsBtn.OnHighlight = delegate
        {
            ItemsBtnRender.material.mainTexture = ItemsBtnStates[1];
        };
        ItemsBtn.OnHighlightEnded = delegate
        {
            ItemsBtnRender.material.mainTexture = ItemsBtnStates[0];
        };
        
        Module.OnActivate = delegate
        {
            MoneyBtnObject.SetActive(false);
            ItemsBtnObject.SetActive(false);
            TakeYourHeart.SetActive(false);
            HoldUpPhase.SetActive(false);
        };
    }

    void Start()
    {
        moduleID = moduleIdCounter++;
        int DownsGen = Random.Range(0, 2);
        switch (DownsGen)
        {
            default:
                {
                    FiveDowns = false;
                    ThreeDownsObj.SetActive(true);
                    Array.Resize(ref AllWeaknesses, 3);
                    break;
                }
            case 1:
                {
                    FiveDowns = true;
                    FiveDownsObj.SetActive(true);
                    Array.Resize(ref AllWeaknesses, 5);
                    break;
                }
        }
        Init();
    }

    void Init()
    {
        MoveTypeRenders[0].material.mainTexture = DefaultMoveIcon;
        MoveTypeRenders[1].material.mainTexture = DefaultMoveIcon;
        MoveTypeRenders[2].material.mainTexture = DefaultMoveIcon;
        MoveTypeRenders[3].material.mainTexture = DefaultMoveIcon;

        StageNr++;
        List<string> PossibleMovesTypes = new List<string>()
        {
        "Fire", "Ice", "Elec", "Wind", "Psy", "Nuclear", "Gun", "Bless", "Curse"
        };
        int CurrentMove = 0;
        int TypesLeft = 9;
        foreach (Renderer Button in MoveTypeRenders)
        {
            int MoveTypeGen = Random.Range(0, TypesLeft);
            string GeneratedMoveType = PossibleMovesTypes[MoveTypeGen];
            PossibleMovesTypes.Remove(GeneratedMoveType);
            int MoveGenerator = Random.Range(0, 4);
            if (GeneratedMoveType == "Fire") //Fire
            {
                MoveTypes[CurrentMove] = "Fire";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Agilao";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Inferno";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Maragidyne";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Blazing Hell";
                            break;
                        }
                }
            } //Fire
            else if (GeneratedMoveType == "Ice")
            {
                MoveTypes[CurrentMove] = "Ice";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Bufula";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Diamond Dust";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Mabufudyne";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Ice Age";
                            break;
                        }
                }
            } //Ice
            else if (GeneratedMoveType == "Elec")
            {
                MoveTypes[CurrentMove] = "Elec";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Zionga";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Thunder Reign";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Maziodyne";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Wild Thunder";
                            break;
                        }
                }
            } //Elec
            else if (GeneratedMoveType == "Wind")
            {
                MoveTypes[CurrentMove] = "Wind";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Garula";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Phanta Rhei";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Magarudyne";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Vacuum Wave";
                            break;
                        }
                }
            } //Wind
            else if (GeneratedMoveType == "Nuclear")
            {
                MoveTypes[CurrentMove] = "Nuclear";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Freila";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Atomic Flare";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Mafreidyne";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Cosmic Flare";
                            break;
                        }
                }
            } //Nuclear
            else if (GeneratedMoveType == "Psy")
            {
                MoveTypes[CurrentMove] = "Psy";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Psio";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Psycho Force";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Mapsiodyne";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Psycho Blast";
                            break;
                        }
                }
            } //Psy
            else if (GeneratedMoveType == "Gun")
            {
                MoveTypes[CurrentMove] = "Gun";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Snap";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "One-Shot Kill";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Triple Down";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Riot Gun";
                            break;
                        }
                }
            } //Gun
            else if (GeneratedMoveType == "Bless")
            {
                MoveTypes[CurrentMove] = "Bless";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Kouga";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Divine\nJudgement";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Makougaon";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Shining Arrows";
                            break;
                        }
                }
            } //Bless
            else if (GeneratedMoveType == "Curse")
            {
                MoveTypes[CurrentMove] = "Curse";
                switch (MoveGenerator)
                {
                    case 0:
                        {
                            Moves[CurrentMove] = "Eiga";
                            break;
                        }
                    case 1:
                        {
                            Moves[CurrentMove] = "Demonic\nDecree";
                            break;
                        }
                    case 2:
                        {
                            Moves[CurrentMove] = "Maeigaon";
                            break;
                        }
                    case 3:
                        {
                            Moves[CurrentMove] = "Abyssal Wings";
                            break;
                        }
                }
            } //Curse
            CurrentMove++;
            TypesLeft--;
        }
        MoveTexts[0].text = Moves[0];
        MoveTexts[1].text = Moves[1];
        MoveTexts[2].text = Moves[2];
        MoveTexts[3].text = Moves[3];

        ShadowGenerator();
    }
    void ShadowGenerator()
    {
        int ShadowTypeGen = Random.Range(0, 4);
        if (MoveTypes[ShadowTypeGen] == "Fire")
        {
            Weakness = "Fire";
            int ShadowWeakToFireGen = Random.Range(0, 6);
            Shadow = ShadowsWeakToFire[ShadowWeakToFireGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Ice")
        {
            Weakness = "Ice";
            int ShadowWeakToIceGen = Random.Range(0, 3);
            Shadow = ShadowsWeakToIce[ShadowWeakToIceGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Elec")
        {
            Weakness = "Elec";
            int ShadowWeakToElecGen = Random.Range(0, 7);
            Shadow = ShadowsWeakToElec[ShadowWeakToElecGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Wind")
        {
            Weakness = "Wind";
            int ShadowWeakToWindGen = Random.Range(0, 4);
            Shadow = ShadowsWeakToWind[ShadowWeakToWindGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Psy")
        {
            Weakness = "Psy";
            int ShadowWeakToPsyGen = Random.Range(0, 3);
            Shadow = ShadowsWeakToPsy[ShadowWeakToPsyGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Nuclear")
        {
            Weakness = "Nuclear";
            int ShadowWeakToNuclearGen = Random.Range(0, 3);
            Shadow = ShadowsWeakToNuclear[ShadowWeakToNuclearGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Gun")
        {
            Weakness = "Gun";
            int ShadowWeakToGunGen = Random.Range(0, 3);
            Shadow = ShadowsWeakToGun[ShadowWeakToGunGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Bless")
        {
            Weakness = "Bless";
            int ShadowWeakToBlessGen = Random.Range(0, 5);
            Shadow = ShadowsWeakToBless[ShadowWeakToBlessGen];
        }
        else if (MoveTypes[ShadowTypeGen] == "Curse")
        {
            Weakness = "Curse";
            int ShadowWeakToCurseGen = Random.Range(0, 2);
            Shadow = ShadowsWeakToCurse[ShadowWeakToCurseGen];
        }
        Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) The shadow is {2}", moduleID, StageNr, Shadow);
        if (MoveTexts[ShadowTypeGen].text == "Demonic\nDecree")
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) The weakness is {2}. The correct move is Demonic Decree", moduleID, StageNr, Weakness);
        }
        else if (MoveTexts[ShadowTypeGen].text == "Divine\nJudgement")
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) The weakness is {2}. The correct move is Divine Judgement", moduleID, StageNr, Weakness);
        }
        else
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) The weakness is {2}. The correct move is {3}", moduleID, StageNr, Weakness, MoveTexts[ShadowTypeGen].text);
        }
        ShadowName.text = Shadow;
        AllWeaknesses[CurrentShadow] = Weakness;
        AllDownedShadows.Add(Shadow);
        CurrentShadow++;
    }

    protected bool Move1()
    {
        Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) Used {2} with type {3}. Desired: {4}", moduleID, StageNr, MoveTexts[0].text, MoveTypes[0], Weakness);
        if (MoveTypes[0] == Weakness)
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was correct.", moduleID, StageNr);
            int RandomAttackSound = Random.Range(0, 3);
            switch (RandomAttackSound)
            {
                default:
                    {
                        BombAudio.PlaySoundAtTransform("Attack1", transform);
                        break;
                    }
                case 1:
                    {
                        BombAudio.PlaySoundAtTransform("Attack2", transform);
                        break;
                    }
                case 2:
                    {
                        BombAudio.PlaySoundAtTransform("Attack3", transform);
                        break;
                    }
            }
            if (!FiveDowns)
            {
                ThreeDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 3)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
            else
            {
                FiveDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 5)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
        }
        else
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was wrong. Strike handed", moduleID, StageNr);
            BombAudio.PlaySoundAtTransform("Incorrect", transform);


            if (MoveTypes[0] == "Fire") //Fire
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[0];
            } //Fire
            else if (MoveTypes[0] == "Ice")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[1];
            } //Ice
            else if (MoveTypes[0] == "Elec")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[2];
            } //Elec
            else if (MoveTypes[0] == "Wind")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[3];
            } //Wind
            else if (MoveTypes[0] == "Nuclear")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[4];
            } //Nuclear
            else if (MoveTypes[0] == "Psy")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[5];
            } //Psy
            else if (MoveTypes[0] == "Gun")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[6];
            } //Gun
            else if (MoveTypes[0] == "Bless")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[7];
            } //Bless
            else if (MoveTypes[0] == "Curse")
            {
                MoveTypeRenders[0].material.mainTexture = MoveTypeIcons[8];
            } //Curse


            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }
    protected bool Move2()
    {
        Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) Used {2} with type {3}. Desired: {4}", moduleID, StageNr, MoveTexts[1].text, MoveTypes[1], Weakness);
        if (MoveTypes[1] == Weakness)
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was correct.", moduleID, StageNr);
            int RandomAttackSound = Random.Range(0, 3);
            switch (RandomAttackSound)
            {
                default:
                    {
                        BombAudio.PlaySoundAtTransform("Attack1", transform);
                        break;
                    }
                case 1:
                    {
                        BombAudio.PlaySoundAtTransform("Attack2", transform);
                        break;
                    }
                case 2:
                    {
                        BombAudio.PlaySoundAtTransform("Attack3", transform);
                        break;
                    }
            }
            if (!FiveDowns)
            {
                ThreeDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 3)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
            else
            {
                FiveDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 5)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
        }
        else
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was wrong. Strike handed", moduleID, StageNr);
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
            if (MoveTypes[1] == "Fire") //Fire
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[0];
            } //Fire
            else if (MoveTypes[1] == "Ice")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[1];
            } //Ice
            else if (MoveTypes[1] == "Elec")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[2];
            } //Elec
            else if (MoveTypes[1] == "Wind")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[3];
            } //Wind
            else if (MoveTypes[1] == "Nuclear")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[4];
            } //Nuclear
            else if (MoveTypes[1] == "Psy")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[5];
            } //Psy
            else if (MoveTypes[1] == "Gun")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[6];
            } //Gun
            else if (MoveTypes[1] == "Bless")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[7];
            } //Bless
            else if (MoveTypes[1] == "Curse")
            {
                MoveTypeRenders[1].material.mainTexture = MoveTypeIcons[8];
            } //Curse

            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }
    protected bool Move3()
    {
        Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) Used {2} with type {3}. Desired: {4}", moduleID, StageNr, MoveTexts[2].text, MoveTypes[2], Weakness);
        if (MoveTypes[2] == Weakness)
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was correct.", moduleID, StageNr);
            int RandomAttackSound = Random.Range(0, 3);
            switch (RandomAttackSound)
            {
                default:
                    {
                        BombAudio.PlaySoundAtTransform("Attack1", transform);
                        break;
                    }
                case 1:
                    {
                        BombAudio.PlaySoundAtTransform("Attack2", transform);
                        break;
                    }
                case 2:
                    {
                        BombAudio.PlaySoundAtTransform("Attack3", transform);
                        break;
                    }
            }
            if (!FiveDowns)
            {
                ThreeDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 3)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
            else
            {
                FiveDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 5)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
        }
        else
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was wrong. Strike handed", moduleID, StageNr);
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
            if (MoveTypes[2] == "Fire") //Fire
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[0];
            } //Fire
            else if (MoveTypes[2] == "Ice")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[1];
            } //Ice
            else if (MoveTypes[2] == "Elec")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[2];
            } //Elec
            else if (MoveTypes[2] == "Wind")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[3];
            } //Wind
            else if (MoveTypes[2] == "Nuclear")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[4];
            } //Nuclear
            else if (MoveTypes[2] == "Psy")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[5];
            } //Psy
            else if (MoveTypes[2] == "Gun")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[6];
            } //Gun
            else if (MoveTypes[2] == "Bless")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[7];
            } //Bless
            else if (MoveTypes[2] == "Curse")
            {
                MoveTypeRenders[2].material.mainTexture = MoveTypeIcons[8];
            } //Curse

            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }
    protected bool Move4()
    {
        Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) Used {2} with type {3}. Desired: {4}", moduleID, StageNr, MoveTexts[3].text, MoveTypes[3], Weakness);
        if (MoveTypes[3] == Weakness)
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was correct.", moduleID, StageNr);
            int RandomAttackSound = Random.Range(0, 3);
            switch (RandomAttackSound)
            {
                default:
                    {
                        BombAudio.PlaySoundAtTransform("Attack1", transform);
                        break;
                    }
                case 1:
                    {
                        BombAudio.PlaySoundAtTransform("Attack2", transform);
                        break;
                    }
                case 2:
                    {
                        BombAudio.PlaySoundAtTransform("Attack3", transform);
                        break;
                    }
            }
            if (!FiveDowns)
            {
                ThreeDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 3)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
            else
            {
                FiveDownsLights[CurrentDown].material.color = new Color32(0, 255, 0, 255);
                CurrentDown++;
                if (CurrentDown == 5)
                {
                    BombAudio.PlaySoundAtTransform("Looking_Cool_Joker", transform);
                    StartCoroutine(HoldUp());
                }
                else
                {
                    Init();
                }
            }
        }
        else
        {
            Debug.LogFormat("[Hold Ups #{0}] (Stage: {1}) ...Which was wrong. Strike handed", moduleID, StageNr);
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
            if (MoveTypes[3] == "Fire") //Fire
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[0];
            } //Fire
            else if (MoveTypes[3] == "Ice")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[1];
            } //Ice
            else if (MoveTypes[3] == "Elec")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[2];
            } //Elec
            else if (MoveTypes[3] == "Wind")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[3];
            } //Wind
            else if (MoveTypes[3] == "Nuclear")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[4];
            } //Nuclear
            else if (MoveTypes[3] == "Psy")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[5];
            } //Psy
            else if (MoveTypes[3] == "Gun")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[6];
            } //Gun
            else if (MoveTypes[3] == "Bless")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[7];
            } //Bless
            else if (MoveTypes[3] == "Curse")
            {
                MoveTypeRenders[3].material.mainTexture = MoveTypeIcons[8];
            } //Curse

            GetComponent<KMBombModule>().HandleStrike();
        }
        return false;
    }

    IEnumerator HoldUp()
    {
        for (int T = 0; T < 2; T++)
        {
            if (T == 1)
            {
                BombAudio.PlaySoundAtTransform("Hold_Up", transform);
                BattlePhase.SetActive(false);
                HoldUpPhase.SetActive(true);
                StartCoroutine(HoldUpAnim());
                DownedShadowGen();
                HoldUpActionGenerator();
            }
            yield return new WaitForSeconds(1);
        }
    }

    void DownedShadowGen()
    {
        int RandomShadowGen;

        if (FiveDowns)
        {
            RandomShadowGen = Random.Range(0, 5);
        }
        else 
        {
            RandomShadowGen = Random.Range(0, 3);
            
        }
        SelectedDownedShadow = AllDownedShadows[RandomShadowGen];
        DownedShadowText.text = SelectedDownedShadow;
        ShadowPersonalityGen();
    }

    void ShadowPersonalityGen()
    {
        List<string> TimidShadows = new List<string>()
        {
            "Pixie", "Incubis", "Agathion", "Andras", "Koropokguru", "Jack Frost", "Inugami", "Orobas", "Isis"
        };
        List<string> GloomyShadows = new List<string>()
        {
            "Jack 'O Lantern", "Silky", "Mokoi", "Onmoraki", "Take-Minakata", "Naga", "Lamia", "Thoth"
        };
        List<string> UpbeatShadows = new List<string>()
        {
            "Mandrake", "Kelpie", "Apsaras", "Hua Po", "Koppa-Tengu", "Makami", "Nekomata"
        };
        List<string> IrritableShadows = new List<string>()
        {
            "Bicorn", "Berith", "Succubus", "Nue", "High Pixie", "Angel", "Orthrus", "Yaksini", "Leanan Sidhe", "Rakshasa", "Sandman", "Anzu"
        };

        if (TimidShadows.Where((x) => x.Contains(SelectedDownedShadow)).Any())
        {
            //Type is Timid
            ShadowPersonalityRender.material.mainTexture = AllShadowPersonalities[0];
            Personality = "Timid";
        }
        else if (GloomyShadows.Where((x) => x.Contains(SelectedDownedShadow)).Any())
        {
            //Type is Gloomy
            ShadowPersonalityRender.material.mainTexture = AllShadowPersonalities[1];
            Personality = "Gloomy";
        }
        else if (UpbeatShadows.Where((x) => x.Contains(SelectedDownedShadow)).Any())
        {
            //Type is Upbeat
            ShadowPersonalityRender.material.mainTexture = AllShadowPersonalities[2];
            Personality = "Upbeat";
        }
        else if (IrritableShadows.Where((x) => x.Contains(SelectedDownedShadow)).Any())
        {
            //Type is Irritable
            ShadowPersonalityRender.material.mainTexture = AllShadowPersonalities[3];
            Personality = "Irritable";
        }
        Debug.LogFormat("[Hold Ups #{0}] (Stage: Hold Up) The selected shadow is {1} (Personality: {2})", moduleID, SelectedDownedShadow, Personality);
    }

    IEnumerator HoldUpAnim()
    {
        HoldUpAnimObject.SetActive(true);
        for (int Frame = 0; Frame < 16; Frame++)
        {
            HoldUpRender.material.mainTexture = HoldUpFrames[Frame];
            yield return new WaitForSeconds(0.05f);
        }
        HoldUpAnimObject.SetActive(false);
    }

    void HoldUpActionGenerator()
    {
        string Indic = string.Join("", BombInfo.GetIndicators().ToArray());
        if (Indic.Any("JOKER".Contains) && Indic.Any("MONA".Contains) && Indic.Any("SKULL".Contains))
        {
            DesiredHoldUpAction = "perform an All-Out Attack";
        }
        else if (AllWeaknesses.Any("Gun".Contains))
        {
            DesiredHoldUpAction = "break formation";
        }
        else
        {
            DesiredHoldUpAction = "negotiate";
        }
        Debug.LogFormat("[Hold Ups #{0}] (Stage: Hold Up) , so the desired Hold Up action is to {1}", moduleID, DesiredHoldUpAction);
    }

    protected bool HandleAllOutAttack()
    {
        if (DesiredHoldUpAction == "perform an All-Out Attack")
        {
            AllOutAttackBtnObject.SetActive(false);
            BreakFormationBtnObject.SetActive(false);
            TalkBtnObject.SetActive(false);

            GetComponent<KMBombModule>().HandlePass();
            BombAudio.PlaySoundAtTransform("Negotiation_Choice", transform);
            StartCoroutine(DelayAllOutAttack());
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
        }
        return false;
    }

    protected bool HandleNegotiation()
    {
        if (DesiredHoldUpAction == "negotiate")
        {
            AllOutAttackBtnObject.SetActive(false);
            BreakFormationBtnObject.SetActive(false);
            TalkBtnObject.SetActive(false);

            MoneyBtnObject.SetActive(true);
            ItemsBtnObject.SetActive(true);
            ComponentRender.material.color = new Color32(120, 120, 120, 120);
            ShadowNameBox.gameObject.transform.localPosition = new Vector3(-0.0183f, 0.014218f, 0.0407f);
            ShadowPersonality.SetActive(true);
            if (Personality == "Upbeat" || Personality == "Irritable")
            {
                Debug.LogFormat("[Hold Ups #{0}] (Stage: Negotiation) The personality is {1}, so the action is to ask for money", moduleID, Personality);
            }
            else
            {
                Debug.LogFormat("[Hold Ups #{0}] (Stage: Negotiation) The personality is {1}, so the action is to ask for items", moduleID, Personality);
            }
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
        }
        return false;
    }

    protected bool HandleMoney()
    {
        if (Personality == "Upbeat" || Personality == "Irritable")
        {
            GetComponent<KMBombModule>().HandlePass();
            ComponentRender.material.color = new Color32(255, 255, 255, 255);
            BombAudio.PlaySoundAtTransform("Negotiation_Choice", transform);
            MoneyBtnObject.SetActive(false);
            ItemsBtnObject.SetActive(false);
            StartCoroutine(DelayAnim());
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
        }
        return false;
    }
    protected bool HandleItems()
    {
        if (Personality == "Timid" || Personality == "Gloomy")
        {
            GetComponent<KMBombModule>().HandlePass();
            ComponentRender.material.color = new Color32(255, 255, 255, 255);
            BombAudio.PlaySoundAtTransform("Negotiation_Choice", transform);
            MoneyBtnObject.SetActive(false);
            ItemsBtnObject.SetActive(false);
            StartCoroutine(DelayAnim());
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
        }
        return false;
    }

    protected bool HandleBreak()
    {
        if (DesiredHoldUpAction == "break formation")
        {
            AllOutAttackBtnObject.SetActive(false);
            BreakFormationBtnObject.SetActive(false);
            TalkBtnObject.SetActive(false);

            GetComponent<KMBombModule>().HandlePass();
            BombAudio.PlaySoundAtTransform("Negotiation_Choice", transform);
            StartCoroutine(DelayAnim());
        }
        else
        {
            GetComponent<KMBombModule>().HandleStrike();
            BombAudio.PlaySoundAtTransform("Incorrect", transform);
        }
        return false;
    }

    IEnumerator DelayAllOutAttack()
    {
        for (int T = 0; T < 3; T++)
        {
            if (T == 2)
            {
                StartCoroutine(AllOutAttackAnim());
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }
    IEnumerator DelayAnim()
    {
        for (int T = 0; T < 3; T++)
        {
            if (T == 2)
            {
                StartCoroutine(SolveTransition());
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }
    IEnumerator AllOutAttackAnim()
    {
        JokerPortrait.SetActive(false);
        ShadowNameBox.SetActive(false);
        ComponentRender.material = RedComponent;
        SolveAnimObject.SetActive(true);
        byte RedColor = 238;
        Color32 color = RedComponent.color;
        BombAudio.PlaySoundAtTransform("AllOutAttackSFX", transform);
        for (int T = 0; T < 108; T++)
        {
            SolveRender.material.mainTexture = AllOutAttackFrames[T];
            yield return new WaitForSecondsRealtime(0.041f);
        }
        for (int T = 108; T < 205; T++)
        {
            SolveRender.material.mainTexture = AllOutAttackFrames[T];
            if (T > 192)
            {
                color.g = 0;
                color.r = RedColor;
                ComponentRender.material.color = color;
                RedColor -= 21;
            }
            yield return new WaitForSecondsRealtime(0.0335f);
        }
        StartCoroutine(FlagAnimation());
    }
    IEnumerator SolveTransition()
    {
        BombAudio.PlaySoundAtTransform("Transit", transform);
        JokerPortrait.SetActive(false);
        ShadowNameBox.SetActive(false);
        ShadowPersonality.SetActive(false);
        ComponentRender.material = RedComponent;
        SolveAnimObject.SetActive(true);
        byte RedColor = 238;
        Color32 color = RedComponent.color;
        for (int T = 0; T < 8; T++)
        {
            color.g = 0;
            color.r = RedColor;
            ComponentRender.material.color = color;
            RedColor -= 30;
            SolveRender.material.mainTexture = TransitFrames[T];
            yield return new WaitForSecondsRealtime(0.04f);
        }
        ComponentRender.material.color = new Color32(0, 0, 0, 0);
        StartCoroutine(FlagAnimation());
    }
    IEnumerator FlagAnimation()
    {
        for (int T = 0; T < 18; T++)
        {
            SolveRender.material.mainTexture = FlagFrames[T];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        for (int T = 17; T > -1; T--)
        {
            SolveRender.material.mainTexture = FlagFrames[T];
            yield return new WaitForSecondsRealtime(0.1f);
        }
        StartCoroutine(FlagAnimation());
    }

    //twitch plays
    private bool paramsValid1(string s, string s2)
    {
        if(s.EqualsIgnoreCase("Blazing") && s2.EqualsIgnoreCase("Hell"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Diamond") && s2.EqualsIgnoreCase("Dust"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Ice") && s2.EqualsIgnoreCase("Age"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Thunder") && s2.EqualsIgnoreCase("Reign"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Wild") && s2.EqualsIgnoreCase("Thunder"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Phanta") && s2.EqualsIgnoreCase("Rhei"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Vacuum") && s2.EqualsIgnoreCase("Wave"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Atomic") && s2.EqualsIgnoreCase("Flare"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Cosmic") && s2.EqualsIgnoreCase("Flare"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Psycho") && s2.EqualsIgnoreCase("Force"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Psycho") && s2.EqualsIgnoreCase("Blast"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("One-Shot") && s2.EqualsIgnoreCase("Kill"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Triple") && s2.EqualsIgnoreCase("Down"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Riot") && s2.EqualsIgnoreCase("Gun"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Divine") && s2.EqualsIgnoreCase("Judgement"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Shining") && s2.EqualsIgnoreCase("Arrows"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Demonic") && s2.EqualsIgnoreCase("Decree"))
        {
            return true;
        }
        else if (s.EqualsIgnoreCase("Abyssal") && s2.EqualsIgnoreCase("Wings"))
        {
            return true;
        }
        return false;
    }

    private bool paramsValid2(string s)
    {
        string[] validMoves = { "agilao", "inferno", "maragidyne", "bufula", "mabufudyne", "zionga", "maziodyne", "garula", "magarudyne", "freila", "mafreidyne", "psio", "mapsiodyne", "snap", "kouga", "makougaon", "eiga", "maeigaon" };
        s = s.ToLower();
        if (!validMoves.Contains(s))
        {
            return false;
        }
        return true;
    }

    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} use <move> [Performs the specified move] | !{0} attack/break/talk/items/money [On a hold up, performs the specified action]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*attack\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*all-out attack\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (HoldUpPhase.activeSelf == true && MoneyBtnObject.activeSelf == false)
            {
                AllOutAttackBtn.OnInteract();
            }
            else
            {
                yield return "sendtochaterror You are not currently in the initial Hold-Up phase! Why perform an 'All-Out Attack'?";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*break\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*break formation\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (HoldUpPhase.activeSelf == true && MoneyBtnObject.activeSelf == false)
            {
                BreakFormationBtn.OnInteract();
            }
            else
            {
                yield return "sendtochaterror You are not currently in the initial Hold-Up phase! Why perform a 'Formation Break'?";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*talk\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*negotiate\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (HoldUpPhase.activeSelf == true && MoneyBtnObject.activeSelf == false)
            {
                TalkBtn.OnInteract();
            }
            else
            {
                yield return "sendtochaterror You are not currently in the initial Hold-Up phase! Why perform a 'Negotiation'?";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*items\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*item\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*I want an item\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (HoldUpPhase.activeSelf == true && MoneyBtnObject.activeSelf == true)
            {
                ItemsBtn.OnInteract();
            }
            else
            {
                yield return "sendtochaterror You are not currently in the negotiation Hold-Up phase! Why 'ask for an item'?";
            }
            yield break;
        }
        if (Regex.IsMatch(command, @"^\s*money\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant) || Regex.IsMatch(command, @"^\s*Give me some money\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            if (HoldUpPhase.activeSelf == true && MoneyBtnObject.activeSelf == true)
            {
                MoneyBtn.OnInteract();
            }
            else
            {
                yield return "sendtochaterror You are not currently in the negotiation Hold-Up phase! Why 'ask for some money'?";
            }
            yield break;
        }

        string[] parameters = command.Split(' ');
        if (Regex.IsMatch(parameters[0], @"^\s*use\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            if (parameters.Length == 3)
            {
                yield return null;
                if (paramsValid1(parameters[1], parameters[2]))
                {
                    if(HoldUpPhase.activeSelf == true)
                    {
                        yield return "sendtochaterror You are not currently in the Knock Down Phase! Why 'perform a move'?";
                    }
                    else
                    {
                        string checker = "";
                        if (parameters[1].EqualsIgnoreCase("Divine") && parameters[2].EqualsIgnoreCase("Judgement"))
                        {
                            checker = "Divine\nJudgement";
                        }
                        else if (parameters[1].EqualsIgnoreCase("Demonic") && parameters[2].EqualsIgnoreCase("Decree"))
                        {
                            checker = "Demonic\nDecree";
                        }
                        else
                        {
                            checker = parameters[1] + " " + parameters[2];
                        }

                        if (Moves[0].EqualsIgnoreCase(checker))
                        {
                            Move1Button.OnInteract();
                        }
                        else if (Moves[1].EqualsIgnoreCase(checker))
                        {
                            Move2Button.OnInteract();
                        }
                        else if (Moves[2].EqualsIgnoreCase(checker))
                        {
                            Move3Button.OnInteract();
                        }
                        else if (Moves[3].EqualsIgnoreCase(checker))
                        {
                            Move4Button.OnInteract();
                        }
                        else
                        {
                            yield return "sendtochaterror This move is not an option right now!";
                        }
                    }
                }
                else
                {
                    yield return "sendtochaterror I do not recognize this kind of move!";
                }
            }
            else if(parameters.Length == 2)
            {
                yield return null;
                if (paramsValid2(parameters[1]))
                {
                    if (HoldUpPhase.activeSelf == true)
                    {
                        yield return "sendtochaterror You are not currently in the Knock Down Phase! Why 'perform a move'?";
                    }
                    else
                    {
                        if (Moves[0].EqualsIgnoreCase(parameters[1]))
                        {
                            Move1Button.OnInteract();
                        }
                        else if (Moves[1].EqualsIgnoreCase(parameters[1]))
                        {
                            Move2Button.OnInteract();
                        }
                        else if (Moves[2].EqualsIgnoreCase(parameters[1]))
                        {
                            Move3Button.OnInteract();
                        }
                        else if (Moves[3].EqualsIgnoreCase(parameters[1]))
                        {
                            Move4Button.OnInteract();
                        }
                        else
                        {
                            yield return "sendtochaterror This move is not an option right now!";
                        }
                    }
                }
                else
                {
                    yield return "sendtochaterror I do not recognize this kind of move!";
                }
            }
            yield break;
        }
    }

    IEnumerator TwitchHandleForcedSolve()
    {
        if (!HoldUpPhase.activeSelf)
        {
            KMSelectable[] moveBtns = new KMSelectable[] { Move1Button, Move2Button, Move3Button, Move4Button };
            int start = StageNr - 1;
            int end = 0;
            if (FiveDowns)
                end = 5;
            else
                end = 3;
            for (int i = start; i < end; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (MoveTypes[j] == Weakness)
                    {
                        moveBtns[j].OnInteract();
                        yield return new WaitForSeconds(0.1f);
                        break;
                    }
                }
            }
        }
        while (!HoldUpPhase.activeSelf) { yield return true; yield return new WaitForSeconds(0.1f); }
        if (DesiredHoldUpAction == "perform an All-Out Attack")
        {
            AllOutAttackBtn.OnInteract();
        }
        else if (DesiredHoldUpAction == "break formation")
        {
            BreakFormationBtn.OnInteract();
        }
        else if (DesiredHoldUpAction == "negotiate" && !MoneyBtnObject.activeSelf)
        {
            TalkBtn.OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
        if (MoneyBtnObject.activeSelf)
        {
            if (Personality == "Upbeat" || Personality == "Irritable")
            {
                MoneyBtn.OnInteract();
            }
            else
            {
                ItemsBtn.OnInteract();
            }
        }
    }
}