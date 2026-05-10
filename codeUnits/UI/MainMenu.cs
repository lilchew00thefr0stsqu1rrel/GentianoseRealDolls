using TowerDefense;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GentianoseRealDolls
{
    public class MainMenu : MonoBehaviour
    {

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_VersionText.text = $"v. {m_Version.Domain} {m_Version.Major}." +
                $"{m_Version.Minor}.{m_Version.Micro}";

            m_HelpPanel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        private int m_LocationIndex;
        public void ReadCurrScene()
        {
            Saver<int>.TryLoad(CurrentScene.fileNameScene, ref m_LocationIndex);
        }

        [SerializeField] private Party party;

        public void ToHabitat()
        {
            Level.SetArriveFromMenu();

            ReadCurrScene();

            SceneHelper.EnterHouse(SceneHelper.LevelToScene(m_LocationIndex));

            party.PlaceDolls(m_LocationIndex, new Vector3(0, 1, 0));
        }



        public void Quit()
        {
            Application.Quit();
        }

        [SerializeField] private VersionData m_Version;
        [SerializeField] private Text m_VersionText;

        [SerializeField] private GameObject m_ButtonsPanel;
        [SerializeField] private GameObject m_HelpPanel;
        public void OpenHelp()
        {
            m_ButtonsPanel.SetActive(false);
            m_HelpPanel.SetActive(true);
        }
        public void CloseHelp()
        {
            m_HelpPanel.SetActive(false);
            m_ButtonsPanel.SetActive(true);
        }
    }
}

