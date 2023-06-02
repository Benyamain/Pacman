using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public int score { get; private set; }
    public int lives { get; private set; }
    public int ghostMultiplier { get; private set; } = 1;
    public Text gameOverText;
    public Text scoreText;
    public Text livesText;
    public Text getReadyText;
    public AudioSource pelletSFX;
    public AudioSource ghostEatenSFX;
    public AudioSource frightenedSFX;
    public AudioSource pacmanDeathSFX;
    public AudioSource getReadySFX;
    private bool canPlay = false;

    private void Awake()
    {
        if (!getReadySFX.isPlaying)
        {
            StartCoroutine(EnablePlayAfterSFX());
        }

        getReadyText.enabled = true;

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private IEnumerator EnablePlayAfterSFX()
    {
        getReadySFX.Play();

        yield return new WaitForSeconds(getReadySFX.clip.length);

        canPlay = true;
    }

    private void Update()
    {
        if (canPlay && Input.anyKeyDown)
        {
            getReadyText.enabled = false;

            if (this.lives <= 0)
            {
                NewGame();
            }
        }
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }

    private void NewRound()
    {
        gameOverText.enabled = false;

        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        ResetGhostMultiplier();

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    // Will be triggered by other scripts so public
    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);

        if (!ghostEatenSFX.isPlaying)
        {
            ghostEatenSFX.Play();
        }
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        pacman.DeathSequence();

        SetLives(this.lives - 1);

        if (!pacmanDeathSFX.isPlaying)
        {
            pacmanDeathSFX.Play();
        }

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(this.score + pellet.Points);

        if (!pelletSFX.isPlaying)
        {
            pelletSFX.Play();
        }

        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        if (!frightenedSFX.isPlaying)
        {
            frightenedSFX.Play();
            frightenedSFX.loop = true;
        }

        // Change ghost state to frightened
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        // Timer in progress will get cancelled and start over again
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
        Invoke(nameof(StopFrightenedSFX), pellet.duration);
    }

    private void StopFrightenedSFX()
    {
        frightenedSFX.Stop();
        frightenedSFX.loop = false;
    }


    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            // Game object is active so there are remaining pellets
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
}
