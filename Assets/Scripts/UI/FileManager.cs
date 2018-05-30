using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class FileManager : MonoBehaviour {

    string streamingAssetsPath;
    public List<string> eligibleFileTypes = new List<string> { ".json", ".txt" };   // File extensions shown in the file selection menu

    public GameManager gameManager;

    private string selectedFilename;

    public GameObject fileModalOpen;
    public GameObject fileModalClosed;
    PanelFiles panelFiles;

    public GameObject fileMenuOpen;
    public GameObject fileMenuClosed;

    void Awake()
    {
        streamingAssetsPath = Application.streamingAssetsPath;
        panelFiles = fileModalOpen.GetComponent<PanelFiles>();
        CloseFileMenu();
    }

    public void SelectFile(string filename)
    {
        selectedFilename = filename;
        panelFiles.filenameInput.text = filename;
        panelFiles.HideErrorMessage();
    }
    
    public void LoadFile()
    {
        if (selectedFilename != null) {

            Debug.Log("Loaded Fixtures file: " + selectedFilename);
            gameManager.LoadNewScene(selectedFilename);
            CloseFileMenu();
            ClosePanel();

        } else {
            panelFiles.ShowErrorMessage("Error: No file selected.");
            Debug.LogError("No file selected.");
        }
    }

    public void SaveFile()
    {
        if (panelFiles.filenameInput.text.Length > 0)
        {
            Debug.Log("Saved Fixtures file: " + panelFiles.filenameInput.text);
            gameManager.SaveSceneAs(panelFiles.filenameInput.text);
            CloseFileMenu();
            ClosePanel();
            // TODO: Confirm overwrite on existing files

        } else
        {
            panelFiles.ShowErrorMessage("Error: Please enter a filename.");
        }
    }

    public void LoadCuesFile()
    {
        if (selectedFilename != null)
        {

            Debug.Log("Loaded Cues file: " + selectedFilename);
            gameManager.cuesManager.LoadCuesFile(Globals.CuesPath + selectedFilename);
            CloseFileMenu();
            ClosePanel();

        }
        else
        {
            panelFiles.ShowErrorMessage("Error: No file selected.");
            Debug.LogError("No file selected.");
        }
    }
    public void SaveCuesFile()
    {
        if (panelFiles.filenameInput.text.Length > 0)
        {
            Debug.Log("Saved Cues file: " + panelFiles.filenameInput.text);
            gameManager.cuesManager.cuesImport.SerializeCuesData(panelFiles.filenameInput.text);
            CloseFileMenu();
            ClosePanel();
            // TODO: Confirm overwrite on existing files
        }
        else
        {
            panelFiles.ShowErrorMessage("Error: Please enter a filename.");
        }
    }

    public void OpenFileMenu()
    {
        fileMenuOpen.SetActive(true);
        fileMenuClosed.SetActive(false);
    }
    public void CloseFileMenu()
    {
        fileMenuOpen.SetActive(false);
        fileMenuClosed.SetActive(true);
    }

    public void OpenModalLoadFixtures()
    {
        CloseFileMenu();
        fileModalOpen.SetActive(true);
        panelFiles.GoToPanelState(PanelFiles.FileMenuState.FixturesLoad);
    }
    public void OpenModalSaveFixtures()
    {
        CloseFileMenu();
        fileModalOpen.SetActive(true);
        panelFiles.GoToPanelState(PanelFiles.FileMenuState.FixturesSave);
    }
    public void OpenModalLoadCues()
    {
        CloseFileMenu();
        panelFiles.GoToPanelState(PanelFiles.FileMenuState.CuesLoad);
        fileModalOpen.SetActive(true);

    }
    public void OpenModalSaveCues()
    {
        CloseFileMenu();
        fileModalOpen.SetActive(true);
        panelFiles.GoToPanelState(PanelFiles.FileMenuState.CuesSave);
    }
    public void ClosePanel()
    {
        SelectFile(null);
        panelFiles.filenameInput.text = null;
        fileModalOpen.SetActive(false);
        //fileModalClosed.SetActive(true);
    }

}
