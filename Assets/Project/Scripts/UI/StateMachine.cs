using UnityEngine;

namespace UI
{
    public sealed class StateMachine : MonoBehaviour
    {
        [Header("��������� �����")] [Tooltip("�����, ������� ����� ����������� ��� �������.")] [SerializeField]
        private GameObject _firstScreen;

        /// <summary> ������� �������� �����. </summary>
        private GameObject _currentScreen;

        /// <summary> ���������� ��������� �������������. </summary>
        private bool _initialized;

        private void Awake()
        {
            // �������� ������������ ����������.
            if (_firstScreen == null)
            {
                Debug.LogError($"[{nameof(StateMachine)}] ��������� ����� �� �������� � {gameObject.name}.", this);
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            Initialize();
        }

        /// <summary>
        /// �������������� �����-������ � ���������� ������ �����.
        /// </summary>
        private void Initialize()
        {
            if (_initialized)
                return;

            ChangeState(_firstScreen);
            _initialized = true;
        }

        /// <summary>
        /// ������ �������� ��������� �� ���������.
        /// </summary>
        /// <param name="nextScreen">�����, ������� ������ ��������.</param>
        public void ChangeState(GameObject nextScreen)
        {
            if (nextScreen == null)
            {
                Debug.LogWarning($"[{nameof(StateMachine)}] ������� ������� ��������� �� null � {gameObject.name}.",
                    this);
                return;
            }

            if (_currentScreen == nextScreen)
                return; // ������� ������ ��������, ���� ����� ��� ��.

            // ������������ ���������� �����
            if (_currentScreen != null && _currentScreen.activeSelf)
                _currentScreen.SetActive(false);

            // ���������� �����
            nextScreen.SetActive(true);
            _currentScreen = nextScreen;
        }

        /// <summary>
        /// ���������, �������� �� ��������� ����� �������.
        /// </summary>
        public bool IsCurrent(GameObject screen) => _currentScreen == screen;
    }
}