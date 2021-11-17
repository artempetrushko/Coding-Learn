using Backend;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unisave.Facades;
using UnityEngine;
using UnityEngine.UI;

public class ProfileBehaviour : MonoBehaviour
{
    #region UI-элементы
    [Header("Экран профиля и его страницы")]
    public GameObject ProfileContainer;
    public GameObject ProfileScreen;
    public GameObject MenuPanel;
    public GameObject ProfilePanel;
    public GameObject TeamLeaderboardPanel;
    public GameObject GeneralLeaderboardPanel;
    public Button BackToMenuButton;   
    [Header("Страница профиля")]
    public Text Nickname;
    public Text TotalScore;
    [Header("Панель со списком команд игрока")]
    public GameObject MyTeamsData;
    public GameObject MyTeams;
    public GameObject TeamLetterPrefab;
    public Text NoTeamsLabel;
    [Header("Форма создания команды")]
    public GameObject CreateTeamPanel;
    public InputField CreatingTeamID;
    public InputField CreatingTeamName;
    public InputField CreatingTeamDescription;
    public Text CreatingTeamStatus;
    [Header("Форма поиска команды")]
    public GameObject JoinTeamPanel;
    public InputField JoinTeamName;
    public InputField JoinTeamInviteCode;
    public Text JoinTeamStatus;
    [Header("Таблицы лидеров")]
    public GameObject GeneralLeaderboard;
    public GameObject TeamLeaderboard;
    public Dropdown TeamsDropdown;
    public GameObject LeadboardLetterPrefab;
    #endregion

    private GameObject previousPanel;
    private GameObject previousMenuButton;
    private Color normalButtonColor = new Color(0.227f, 0.227f, 0.227f);
    private Color pressedButtonColor = new Color(0.11f, 0.404f, 0.11f);

    public IEnumerator ShowProfileScreen_COR()
    {
        ProfileContainer.SetActive(true);
        ProfileScreen.GetComponent<Animator>().Play("MoveProfileDown");
        BackToMenuButton.GetComponent<Animator>().Play("MoveBackToMenuButtonUp");
        yield return new WaitForSeconds(0.583f);
        ProfilePanel.SetActive(true);
        ShowProfile();
    }

    public IEnumerator HideProfileScreen_COR()
    {
        ProfileScreen.GetComponent<Animator>().Play("MoveProfileUp");
        BackToMenuButton.GetComponent<Animator>().Play("MoveBackToMenuButtonDown");
        yield return new WaitForSeconds(0.583f);
        ProfilePanel.SetActive(false);
        TeamLeaderboardPanel.SetActive(false);
        GeneralLeaderboardPanel.SetActive(false);
        ProfileContainer.SetActive(false);
        DeleteTable(MyTeams);
    }

    public async void ShowProfile()
    {
        SwitchPanel(ProfilePanel, 0);
        var player = await OnFacet<PlayerDataFacet>.CallAsync<PlayerEntity>(
            nameof(PlayerDataFacet.GetAuthorizedPlayer));
        Nickname.text = player.Nickname;
        TotalScore.text = player.TotalScore.ToString();
        if (player.Teams.Count == 0)
            NoTeamsLabel.gameObject.SetActive(true);
        else FillPLayerTeamsTable(player);
    }

    public void ShowCreatingTeamForm() => StartCoroutine(ShowSecondaryTeamPanel(CreateTeamPanel));

    public void ShowJoinTeamForm() => StartCoroutine(ShowSecondaryTeamPanel(JoinTeamPanel));

    public void ReturnToMyTeamsFrom_CreatingTeamForm() => StartCoroutine(ReturnToPrimaryTeamPanel(CreateTeamPanel));

    public void ReturnToMyTeamsFrom_JoinTeamForm() => StartCoroutine(ReturnToPrimaryTeamPanel(JoinTeamPanel));

    public async void ShowTeamLeaderboard()
    {
        SwitchPanel(TeamLeaderboardPanel, 1);
        var teams = await OnFacet<PlayerDataFacet>.CallAsync<List<TeamEntity>>(
            nameof(PlayerDataFacet.GetPlayerTeams));
        foreach (var team in teams)
            TeamsDropdown.options.Add(new Dropdown.OptionData(team.Title));
        TeamsDropdown.value = 0;
        TeamsDropdown.captionText.text = teams[0].Title;
        ChangeLeaderboardByDropdownOption();
    }

    public async void ShowGeneralLeaderboard()
    {
        SwitchPanel(GeneralLeaderboardPanel, 2);
        var leaderboardData = await OnFacet<PlayerDataFacet>.CallAsync<List<Tuple<string, int>>>(
            nameof(PlayerDataFacet.GetGeneralLeaderboard));
        FillLeaderboard(GeneralLeaderboard, leaderboardData);
    }

    public async void ChangeLeaderboardByDropdownOption()
    {
        DeleteTable(TeamLeaderboard);
        var leaderboardData = await OnFacet<PlayerDataFacet>.CallAsync<List<Tuple<string, int>>>(
            nameof(PlayerDataFacet.GetTeamLeaderboardByOrderNumber),
            TeamsDropdown.value);
        FillLeaderboard(TeamLeaderboard, leaderboardData);

    }

    public async void CreateTeam()
    {
        CreatingTeamStatus.text = "Выполнение...";
        var response = await OnFacet<TeamConnectionFacet>.CallAsync<CreatingTeamResponse>(
            nameof(TeamConnectionFacet.CreateTeam),
            CreatingTeamID.text,
            CreatingTeamName.text,
            CreatingTeamDescription.text);
        switch (response)
        {
            case CreatingTeamResponse.IdExists:
                CreatingTeamStatus.text = "Данный ID уже занят!";
                return;
            case CreatingTeamResponse.InvalidID:
                CreatingTeamStatus.text = "Введён некорректный ID! Возможно, пропущена @ в начале.";
                return;
            case CreatingTeamResponse.NullID:
                CreatingTeamStatus.text = "ID является обязательным!";
                return;
            case CreatingTeamResponse.NullTitle:
                CreatingTeamStatus.text = "Имя является обязательным!";
                return;
            case CreatingTeamResponse.NullDescription:
                CreatingTeamStatus.text = "Описание является обязательным!";
                return;
            case CreatingTeamResponse.OK:
                CreatingTeamStatus.text = "Команда создана!";
                break;
        }
        StartCoroutine(ReturnToPrimaryTeamPanel(CreateTeamPanel));
        NoTeamsLabel.gameObject.SetActive(false);
        CreatingTeamID.text = "";
        CreatingTeamName.text = "";
        CreatingTeamDescription.text = "";
        CreatingTeamStatus.text = "";
        FillPLayerTeamsTable();
    }

    public async void JoinTeam()
    {
        JoinTeamStatus.text = "Выполнение...";
        var response = await OnFacet<TeamConnectionFacet>.CallAsync<JoinTeamResponse>(
            nameof(TeamConnectionFacet.JoinTeam),
            JoinTeamName.text,
            JoinTeamInviteCode.text);
        switch (response)
        {
            case JoinTeamResponse.NullID:
                JoinTeamStatus.text = "Введите ID!";
                return;
            case JoinTeamResponse.NullInviteCode:
                JoinTeamStatus.text = "Введите инвайт-код!";
                return;
            case JoinTeamResponse.InvalidTeamID:
                JoinTeamStatus.text = "Команды с таким ID не существует!";
                return;
            case JoinTeamResponse.InvalidInviteCode:
                JoinTeamStatus.text = "Неверный код!";
                return;
            case JoinTeamResponse.PlayerExists:
                JoinTeamStatus.text = "Вы уже состоите в этой команде!";
                return;
            case JoinTeamResponse.OK:
                JoinTeamStatus.text = "Вы стали участником команды!";
                break;
        }
        StartCoroutine(ReturnToPrimaryTeamPanel(JoinTeamPanel));
        NoTeamsLabel.gameObject.SetActive(false);
        JoinTeamName.text = "";
        JoinTeamInviteCode.text = "";
        FillPLayerTeamsTable();
    }

    private async void FillPLayerTeamsTable(PlayerEntity player = null)
    {
        DeleteTable(MyTeams);
        if (player == null)
            player = await OnFacet<PlayerDataFacet>.CallAsync<PlayerEntity>(
            nameof(PlayerDataFacet.GetAuthorizedPlayer));
        var teams = player.Teams.OrderBy(team => team.Title).ToList();
        for (var i = 0; i < teams.Count; i++)
        {
            var teamLetter = Instantiate(TeamLetterPrefab, MyTeams.transform);
            teamLetter.transform.GetChild(0).GetComponent<Text>().text = player.Teams[i].Title;
            if (player.Teams[i].Owner.Nickname == player.Nickname)
            {
                teamLetter.transform.GetChild(2).gameObject.SetActive(true);
                //teamLetter.transform.GetChild(2).GetComponent<Button>().onClick.AddListener()
            }
            else teamLetter.transform.GetChild(1).gameObject.SetActive(true);
        }
    }

    private IEnumerator ShowSecondaryTeamPanel(GameObject teamPanel)
    {
        MyTeamsData.GetComponent<Animator>().Play("MoveTeamsPanel_MiddleToTop");
        teamPanel.SetActive(true);
        teamPanel.GetComponent<Animator>().Play("MoveTeamsPanel_BottomToMiddle");
        yield break;
    }

    private IEnumerator ReturnToPrimaryTeamPanel(GameObject teamPanel)
    {
        MyTeamsData.GetComponent<Animator>().Play("MoveTeamsPanel_TopToMiddle");
        teamPanel.GetComponent<Animator>().Play("MoveTeamsPanel_MiddleToBottom");
        yield return new WaitForSeconds(0.583f);
        teamPanel.SetActive(false);
    }

    private void SwitchPanel(GameObject nextPanel, int nextButtonNumber)
    {
        if (previousPanel != null)
        {
            previousPanel.SetActive(false);
            previousMenuButton.GetComponent<Image>().color = normalButtonColor;
            previousMenuButton.GetComponent<Button>().enabled = true;
            if (previousPanel == TeamLeaderboardPanel)
            {
                DeleteTable(TeamLeaderboard);
                TeamsDropdown.options.Clear();
            }
            else if (previousPanel == GeneralLeaderboardPanel)
            {
                DeleteTable(GeneralLeaderboard);
            }
        }
        nextPanel.SetActive(true);    
        var nextButton = MenuPanel.transform.GetChild(nextButtonNumber).gameObject;
        nextButton.GetComponent<Image>().color = pressedButtonColor;
        nextButton.GetComponent<Button>().enabled = false;
        previousPanel = nextPanel;
        previousMenuButton = nextButton;       
    }

    private void FillLeaderboard(GameObject leaderboard, List<Tuple<string, int>> leaderboardData)
    {     
        for (var i = 0; i < leaderboardData.Count; i++)
        {
            var newLetter = Instantiate(LeadboardLetterPrefab, leaderboard.transform);
            newLetter.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            newLetter.transform.GetChild(1).GetComponent<Text>().text = leaderboardData[i].Item1;
            newLetter.transform.GetChild(2).GetComponent<Text>().text = leaderboardData[i].Item2.ToString();
        }
        var leaderboardLettersCount = leaderboard.transform.childCount;
        leaderboard.transform.GetChild(0).GetComponent<Image>().color = new Color(0.651f, 0.549f, 0f);
        if (leaderboardLettersCount > 1)
            leaderboard.transform.GetChild(1).GetComponent<Image>().color = new Color(0.494f, 0.494f, 0.494f);
        if (leaderboardLettersCount > 2)
            leaderboard.transform.GetChild(2).GetComponent<Image>().color = new Color(0.804f, 0.498f, 0.196f);
    }

    private void DeleteTable(GameObject table)
    {
        for (var i = 0; i < table.transform.childCount; i++)
            Destroy(table.transform.GetChild(i).gameObject);
    }
}
