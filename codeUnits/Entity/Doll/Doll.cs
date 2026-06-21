using Common;
using SpaceShooter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GentianoseRealDolls
{
    public enum ToiletStat
    {
        Poo,
        AnalSpray,
        Pee,
        Bath,
        BrushTeeth
    }
    /// <summary>
    /// Модель играбельного персонажа
    /// В отличие от других подобных игр
    /// здесь мы управляем не человеком,
    /// а маломорфным животным
    /// Главным героем является серый бамбуковый лемур
    /// Все подобные звери называются куклами
    /// Кукла - не потому что "без сердца"
    /// Кукла - потому что милый
    /// </summary>
    [RequireComponent(typeof(Destructible))]
    public class Doll : MonoBehaviour
    {
        private AllDollCharacters allDolls;
        private AllDollPositions allPositions;

        [SerializeField] private DollScaleValues m_ScaleValues;
        [SerializeField] private DollPosition m_Positions;

        [SerializeField] private Inventory m_Inventory;

        private const long TicksInSecond = 10000000;
        private long StatsPeriod = 9;
        [SerializeField] private DollController m_Controller;
        public DollController DollController => m_Controller;


        public const float sprayAmountInCare = 2.0f;
        private const string fileName = "doll.dat";


        public const int MaxBath = 40;
        public const int MaxBrushTeeth = 30;
        public const int MaxStat = 100;

        public const int MaxLooStat = 10;


        // Вся струя, если 11 - это та, что нужна для прочистки
        public const float TotalAnalGlandVolumeQuotient = 2.618f;

        public const float FullVolumeAnalGlandUnified = TotalAnalGlandVolumeQuotient * MaxLooStat;

        private long TicksInMinute = 600000000;

        [Header("Meta of a Doll")]
        [SerializeField] private int m_DollID;
        public int DollID => m_DollID;

        [SerializeField] private string m_CharacterName;
        public string CharacterName => m_CharacterName;

        [SerializeField] private DollAsset m_Asset;
        public DollAsset Asset { get { return m_Asset; } }
        [SerializeField] private string m_DollSpecies;
        public string DollSpecies => m_DollSpecies;
        #region Data_Pet

        [Header("At home")]

        [Range(0, 40)]
        [SerializeField] private int m_Bath;

        [Range(0, 30)]
        [SerializeField] private int m_BrushTeeth;

        [Range(0, 10)]
        [SerializeField] private int m_LooSpray;
        public int AnalGlandHealth => m_LooSpray;

        [Range(0, 10)]
        [SerializeField] private int m_LooPee;

        [Range(0, 10)]
        [SerializeField] private int m_LooPoo;
        public int PooPoints => m_LooPoo;

        [Range(0, 100)]
        [SerializeField] private int m_FoodHunger;
        public int FoodHunger => m_FoodHunger;

        [Range(0, 100)]
        [SerializeField] private int m_Sleep;
        public int Sleep => m_Sleep;

        [Range(0, 100)]
        [SerializeField] private float m_Joy;
        public float Joy => m_Joy;

        public float Loo => m_LooPee + m_LooPoo + m_LooSpray;
        public float Bathroom => Loo + m_Bath + m_BrushTeeth;


        public bool FullSleep => m_Sleep >= MaxStat;

        #endregion

        #region Data_Combat

       

        private GRDTimer t;

       // private bool m_IsSleeping;
        public enum Attribute
        {
            Water,
            Wind,
            Fire,
            Earth
        }
        public enum ChemicalClass
        {
            Glycoside,
            Alkaloid,
            Polyphenol,
            Inorganic
        }

        [Header("In Dolls City")]

        // В десятых долях миллилитра
        [SerializeField] private int m_AnalGlandVolume;
        public int AnalGlandVolume => m_AnalGlandVolume;
        //[Range(0, 18)]

        // В десятых долях миллилитра
        [SerializeField] private int m_AnalSprayAmount;
        public int AnalSprayAmount => m_AnalSprayAmount;

        [SerializeField] private Attribute m_Attribute;

        [SerializeField] private ChemicalClass m_ChemicalClass;

        [SerializeField] private string m_Chemical;

        [SerializeField] private int m_SprayDamage;  // AOE 


        #endregion

        #region Data_Media

        [Tooltip("0 - звук FX при обычной атаке, 1 - звук FX при заряженной атаке, 2 - возглас при обычной атаке" +
            "3 - звук FX при флемене, 4 - звук животного при флемене, 5 - реплика при флемене " +
            "6 - FX фуньки, 7 - звук животного при фуньке, 8 - реплика при фуньке" +
            "9 - звук мочеиспускания")]

        [SerializeField] private AudioSource[] m_Sounds;

        public AudioSource[] Sounds => m_Sounds;

        [SerializeField] private GameObject m_PoopPrefab;
        [SerializeField] private Turret m_Anus;
        public Turret AnusNipplesTurret => m_Anus;
        [SerializeField] private AIPointPatrol m_Wisp;
        public AIPointPatrol Wisp => m_Wisp;

        public bool PooStart { get; internal set; }

        [SerializeField] private Animator m_Animator;



        [Tooltip("quadrupedal and bipedal forms")]
        public Vector3[] centerOfMassVersions;



        #endregion

        [SerializeField] private SpaceShip petAsSpaceShip;
        public SpaceShip PetAsSpaceShip => petAsSpaceShip;
        private Rigidbody m_rb;
        public Rigidbody RB => m_rb;
        [SerializeField] private Transform m_CoM;

        public static long PreviousTime;

        public static long CurrentTime;

        private event Action OnDollJumps;



        [Tooltip("0 - shit, 1 - spray, 2 - pee, 3 - bath, 4 - brushTeeth")]
        private int[] m_ToiletStats;
        public int[] ToiletStats => m_ToiletStats;

        [SerializeField] private int m_DollSize;
        public int DollSize => m_DollSize;  

        [SerializeField] private bool m_IsActiveDollInParty;
        public bool ActiveDollInPartyStatus => m_IsActiveDollInParty;
        public void SetActiveDoll(bool active)
        {
            m_IsActiveDollInParty = active;
        }

        public void ConstructDoll(Inventory inventory)
        {
            m_Inventory = inventory;
        }


        public const int LocationsNumber = 3;
        #region Unity Event
        private void Awake()
        {
            print("Hello, I'm " + m_CharacterName);

            m_rb = GetComponent<Rigidbody>();

            m_Controller = GetComponent<DollController>();

            m_Stats = new int[8];
            m_CombatStats = new int[2];

            m_CombatStats[0] = m_DollID;
            m_CombatStats[1] = petAsSpaceShip.HitPoints;

        }

        private void OnDestroy()
        {
            print($"Goodbye, Interface 336n! Your {m_CharacterName}");

        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            print(Loo);
            petAsSpaceShip = GetComponent<SpaceShip>();
        }


        #endregion

        #region Stats Storage


        [SerializeField] private int[] m_Stats;
        [SerializeField] private int[] m_CombatStats;
        public static Action<int[]> OnSave;
        public static Action<int[]> OnSaveCombat;

        // Сохранить
        [Tooltip("int[8]")]
        public int[] FetchStats()
        {
            m_Stats[0] = m_DollID;
            m_Stats[1] = m_LooPoo;
            m_Stats[2] = m_AnalSprayAmount;
            m_Stats[3] = m_LooPee;
            m_Stats[4] = m_Bath;
            m_Stats[5] = m_BrushTeeth;

            m_Stats[6] = m_FoodHunger;
            m_Stats[7] = m_Sleep;

            return m_Stats;
        }

        public int GetSprayCarePoints()
        {
            // Формула Фуньки
            m_LooSpray = (int)(MaxLooStat * (1 -
                (m_AnalSprayAmount - m_AnalGlandVolume * 0.8f)
                / (m_AnalGlandVolume * 0.2f)));

            m_LooSpray = Mathf.Clamp(m_LooSpray, 0, MaxLooStat);

            return m_LooSpray;
        }

        [Tooltip("int[8]")]
        public void FillStats(int[] stats)
        {
            m_LooPoo = stats[1];
            m_AnalSprayAmount = stats[2];
            m_LooPee = stats[3];
            m_Bath = stats[4];
            m_BrushTeeth = stats[5];
            m_FoodHunger = stats[6];
            m_Sleep = stats[7];

            m_LooSpray = GetSprayCarePoints();

            

            m_ToiletStats = stats[1..6];
        }


        /// <summary>
        /// Строка имеет вид SSXXXXXXXXYYYYYYYYZZZZZZZZ,
        /// где SS - сцена,
        /// XXXXXXXX - координата в метрах с десятыми (с запятой) и знаком +/-:
        /// например, -14300,5 - на запад на 14 км 300 м 50 см
        /// Аналогично с другими координатами
        /// </summary>
        /// <param name="address"></param>



        #endregion

        #region Service Data Doll





        #endregion


        #region Public API

        // Уменьшение значений шкал со временем
        public void ChangeSleep(bool isSleeping)
        {
            if (isSleeping)
            {
                if (m_Sleep < MaxStat)
                    m_Sleep += 1;
            }
            else
            {
                if (m_Sleep > 0)
                    m_Sleep -= 1;
            }

            //SaveStats();
        }



        /// <summary>
        /// /// Фунька добавляется 0.1 мл (1 очко) каждые 15 мин
        /// Туалет (кал и моча): 1 каждые 15 мин
        /// Ванная: 1 каждые 6 минут 
        /// Чистка зубов: 1 каждые 5 мин
        /// Еда и сон: 1 каждую минуту
        /// </summary>

        public void ReduceStats()
        {

            if (m_AnalSprayAmount < m_AnalGlandVolume)
                m_AnalSprayAmount++;
            if (m_AnalSprayAmount > m_AnalGlandVolume)
                m_AnalSprayAmount = m_AnalGlandVolume;

            m_ScaleValues.AnalSprayAmount = m_AnalSprayAmount;

            // Формула Фуньки
            m_LooSpray = (int)(MaxLooStat * (1 - 
                (m_AnalSprayAmount - m_AnalGlandVolume * 0.8f)
                / (m_AnalGlandVolume * 0.2f)));

            m_LooSpray = Mathf.Clamp(m_LooSpray, 0, MaxLooStat);

            if (m_LooPoo > 0)
                m_LooPoo--;

            if (m_LooPee > 0)
                m_LooPee--;

            if (m_Bath> 0)
                m_Bath--;

            if (m_BrushTeeth > 0)
                m_BrushTeeth--;

            m_ToiletStats = new int[5]
            {
                m_LooPoo,
                m_LooSpray,
                m_LooPee,
                m_Bath,
                m_BrushTeeth
            };

            if (m_FoodHunger > 0)
                m_FoodHunger--;


            FetchStats();

            print(PreviousTime);
        }




        //Установка значений

        public void SetSleep(int sleep)
        {
            m_Sleep = Mathf.Clamp(sleep, 0, MaxStat);
            FetchStats();
        }
        public void SetFoodHunger(int foodHunger)
        {
            m_FoodHunger = Mathf.Clamp(foodHunger, 0, MaxStat);
            FetchStats();
        }



        public void Eat(InventoryItem food)
        {
            m_Inventory.AddItemInstances(food, -1);
            m_FoodHunger = Mathf.Min(m_FoodHunger + food.foodBonus, MaxStat);


            FetchStats();
        }

        public void OhPoop()
        {
            m_LooPoo = 0;
        }

        private void InitToiletStatArray()
        {
            m_ToiletStats = new int[5];
            m_ToiletStats[0] = m_LooPoo;
            m_ToiletStats[1] = m_LooSpray;
            m_ToiletStats[2] = m_LooPee;
            m_ToiletStats[3] = m_Bath;
            m_ToiletStats[4] = m_BrushTeeth;
        }

        public float TakeToiletStat(int index)
        {
            InitToiletStatArray();
            return m_ToiletStats[index];
        }

        public void CareToiletStat(ToiletStat stat, int value)
        {

            if (stat == ToiletStat.Poo)
            {
                m_LooPoo += value;
                m_LooPoo = Mathf.Clamp(m_LooPoo, 0, MaxLooStat);
            }
            if (stat == ToiletStat.AnalSpray)
            {
                m_AnalSprayAmount -= value;
                m_AnalSprayAmount = Mathf.Clamp(m_AnalSprayAmount, 0, AnalGlandVolume);

                // Формула Фуньки
                m_LooSpray = (int)(MaxLooStat * (1 -
                    (m_AnalSprayAmount - m_AnalGlandVolume * 0.8f)
                    / (m_AnalGlandVolume * 0.2f)));

                m_LooSpray = Mathf.Clamp(m_LooSpray, 0, MaxLooStat);


                m_LooSpray = Mathf.Clamp(m_LooSpray, 0, MaxLooStat);
            }
            if (stat == ToiletStat.Pee)
            {
                m_LooPee += value;
                m_LooPee = Mathf.Clamp(m_LooPee, 0, MaxLooStat);
            }
            if (stat == ToiletStat.Bath)
            {
                m_Bath += value;
                m_Bath = Mathf.Clamp(m_Bath, 0, MaxBath);
            }
            if (stat == ToiletStat.BrushTeeth)
            {
                m_BrushTeeth += value;
                m_BrushTeeth = Mathf.Clamp(m_BrushTeeth, 0, MaxBrushTeeth);
            }

            InitToiletStatArray();
            FetchStats();
        }

        public void SetToiletStats(int poo, int analSpray, int pee, int bath, int brushTeeth)
        {
            m_LooPoo = Mathf.Clamp(poo, 0, MaxLooStat);

            m_AnalSprayAmount = Mathf.Clamp(analSpray, 0, AnalGlandVolume);

            // Формула Фуньки
            m_LooSpray = (int)(MaxLooStat * (1 -
                (m_AnalSprayAmount - m_AnalGlandVolume * 0.8f)
                / (m_AnalGlandVolume * 0.2f)));

            m_LooSpray = Mathf.Clamp(m_LooSpray, 0, MaxLooStat);


            m_LooPee = Mathf.Clamp(pee, 0, MaxLooStat);

            m_Bath = Mathf.Clamp(bath, 0, MaxBath);

            m_BrushTeeth = Mathf.Clamp(brushTeeth, 0, MaxBrushTeeth);


            InitToiletStatArray();
            FetchStats();
        }


       

        #endregion


        [Tooltip("0 - база, 1 - шаг, 2 - рысь/кентер, 3 - галоп, 4 - фунька, 5 - какание, 6 - поднять хвост")]
        [Range(0f, 6f)]
        int state = 0;
        public int State
        {
            get { return state; }
            set
            {
                if (value >= 0 &&  value <= 6) 
                    state = value;
            }
        }

        //[Range(1f, 3f)]
        // public int gaitState;
        #region Pet Care
        private Action PooCareLevelFull()
        {
            return () =>
            {
                m_LooPoo = 11;
            };

        }

        #endregion

        #region CharacterSkills
    
        

     


        public void RestoreHP(int hp)
        {
          
            petAsSpaceShip.RestoreHitPoints(Mathf.Min(petAsSpaceShip.MaxHitPoints - petAsSpaceShip.HitPoints, hp));
        }

        public void SetToiletStat(int statID, int statValue)
        {
            InitToiletStatArray();

            m_ToiletStats[statID] = statValue;
        }

        public void FillCombatStats(int[] stats)
        {
            petAsSpaceShip.FillHitPoints(stats[1]);
        }

        public int[] FetchCombatStats()
        {
            m_CombatStats[0] = m_DollID;
            m_CombatStats[1] = petAsSpaceShip.HitPoints;

            return m_CombatStats;
        }

        #endregion

    }

}
