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
    public int apple { get; private set; }
    public int bell { get; private set; }
    public int cherry { get; private set; }
    public int galaxianStarship { get; private set; }
    public int key { get; private set; }
    public int strawberry { get; private set; }
    public int orange { get; private set; }
    public int melon { get; private set; }
    public int ghostMultiplier { get; private set; } = 1;
    public Text gameOverText;
    public Text scoreText;
    public Text livesText;
    public Text appleText;
    public Text bellText;
    public Text cherryText;
    public Text melonText;
    public Text galaxianStarshipText;
    public Text strawberryText;
    public Text keyText;
    public Text orangeText;
    public Text getReadyText;
    public AudioSource pelletSFX;
    public AudioSource ghostEatenSFX;
    public AudioSource frightenedSFX;
    public AudioSource pacmanDeathSFX;
    public AudioSource getReadySFX;
    public AudioSource fruitEatenSFX;
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
        SetApples(0);
        SetBells(0);
        SetCherries(0);
        SetGalaxianStarship(0);
        SetMelons(0);
        SetOranges(0);
        SetKeys(0);
        SetStrawberries(0);
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

    private void SetApples(int apple)
    {
        this.apple = apple;
        appleText.text = "x" + apple.ToString();
    }

    private void SetBells(int bell)
    {
        this.bell = bell;
        bellText.text = "x" + bell.ToString();
    }

    private void SetCherries(int cherry)
    {
        this.cherry = cherry;
        cherryText.text = "x" + cherry.ToString();
    }

    private void SetGalaxianStarship(int galaxianStarship)
    {
        this.galaxianStarship = galaxianStarship;
        galaxianStarshipText.text = "x" + galaxianStarship.ToString();
    }

    private void SetStrawberries(int strawberry)
    {
        this.strawberry = strawberry;
        strawberryText.text = "x" + strawberry.ToString();
    }

    private void SetKeys(int key)
    {
        this.key = key;
        keyText.text = "x" + key.ToString();
    }

    private void SetOranges(int orange)
    {
        this.orange = orange;
        orangeText.text = "x" + orange.ToString();
    }

    private void SetMelons(int melon)
    {
        this.melon = melon;
        melonText.text = "x" + melon.ToString();
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

        // Cherry
        if (pellet.Points == 100) {
            fruitEatenSFX.Play();
            SetCherries(this.cherry + 1);
        }
        // Strawberry
        else if (pellet.Points == 300) {
            fruitEatenSFX.Play();
            SetStrawberries(this.strawberry + 1);
        }
        // Orange
        else if (pellet.Points == 500) {
            fruitEatenSFX.Play();
            SetOranges(this.orange + 1);
        }
        // Apple
        else if (pellet.Points == 700) {
            fruitEatenSFX.Play();
            SetApples(this.apple + 1);
        }
        // Melon
        else if (pellet.Points == 1000) {
            fruitEatenSFX.Play();
            SetMelons(this.melon + 1);
        }
        // Galaxian Starship
        else if (pellet.Points == 2000) {
            fruitEatenSFX.Play();
            SetGalaxianStarship(this.galaxianStarship + 1);
        }
        // Bell
        else if (pellet.Points == 3000) {
            fruitEatenSFX.Play();
            SetBells(this.bell + 1);
        }
        // Key
        else if (pellet.Points == 5000) {
            fruitEatenSFX.Play();
            SetKeys(this.key + 1);
        }

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
