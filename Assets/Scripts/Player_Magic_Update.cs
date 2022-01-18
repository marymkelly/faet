using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Magic_Update : MonoBehaviour
{

    
    public float maxMagic = 100f;
    public float currentMagic;


    // [HideInInspector]
    public int magicLevel = 1; 

    // [HideInInspector]
    public float magicPower = 10f; 

    [HideInInspector]
    public int lifetimeFlora = 0; //lifetime flora collected
    [HideInInspector]
    public int nextLevelFlora; //total flora needed to next level;
    [HideInInspector]
    public int currentFloraRound; //current number of flora needed between levels (e.g. 14)

    private int bonus = 5;
    private int floraMultiplier = 4;

    private bool isCounting = false;
    private bool isRestoring = false;

    public Player_MagicBar magicBar;

    void Start()
    {
        currentMagic = maxMagic;
        
        magicBar.SetMaxMagic(maxMagic);
        magicBar.SetMagic(currentMagic);
        magicBar.SetMagicLevel(magicLevel);

        if(lifetimeFlora == 0) {
            nextLevelFlora = magicLevel * floraMultiplier;
            currentFloraRound = nextLevelFlora - lifetimeFlora;
        }
        
        if(magicPower == 0) {
            magicPower = 10f;
        }
    }

    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Z)) { //for testing purposes
        //     UseMagic(10);
        // }

        if(magicBar.slider.maxValue != maxMagic) {
            magicBar.SetMaxMagic(maxMagic);
        }

        if(magicBar.slider.value != currentMagic) {
            magicBar.SetMagic(currentMagic);
        }

        if(magicBar.magicbarLvl != magicLevel) {
            magicBar.SetMagicLevel(magicLevel);
        }
    }

    public void UseMagic(int damage) {
        currentMagic -= damage;

        CancelCoroutines();

        if(currentMagic <= 0) {
            currentMagic = 0;
            magicBar.SetMagic(currentMagic);
            StartCoroutine("RestoreMagic");
        } else {
            magicBar.SetMagic(currentMagic);
            StartCoroutine("CastCounter");
        }
    }

    public float GetCurrentMagic() {
        return currentMagic;
    }

    void CollectFlora() {
        lifetimeFlora++;
        FindObjectOfType<AudioManager>().Play("Collectable");
        
        if(lifetimeFlora == nextLevelFlora){
            magicLevel++;
            FindObjectOfType<AudioManager>().Play("MagicLevel");
            magicBar.SetMagicLevel(magicLevel);

            if(magicLevel <= 3) {
                bonus = 5;
                floraMultiplier = 4;

                currentFloraRound = (magicLevel * floraMultiplier);
                // nextLevelFlora = (lifetimeFlora + ((magicLevel * floraMultiplier)));

                magicPower += (float)magicLevel;
            } else if(magicLevel > 3 && magicLevel <= 6) {
                bonus = 4;
                floraMultiplier = 2;

                currentFloraRound += ((magicLevel - 3) * floraMultiplier);
                // nextLevelFlora = (lifetimeFlora + ((magicLevel - 3) * floraMultiplier)); 
                magicPower += (magicLevel / 2f);
            } else {
                bonus = 3;
                floraMultiplier = 1;

                currentFloraRound += ((magicLevel - 6) * floraMultiplier);
                // nextLevelFlora = (lifetimeFlora + ((magicLevel - 6) * floraMultiplier));
                magicPower += (magicLevel / 2f);
            }

            nextLevelFlora = lifetimeFlora + currentFloraRound;
            // currentFloraRound = nextLevelFlora - lifetimeFlora;

            maxMagic += (4 * bonus);
            currentMagic = maxMagic;

            magicBar.SetMaxMagic(maxMagic);
            magicBar.SetMagic(currentMagic);

         
        } else {
            if(currentMagic < maxMagic) {
                currentMagic += Mathf.Ceil((maxMagic * 0.1f));
                magicBar.SetMagic(currentMagic);
            } else {
                currentMagic = maxMagic;
                magicBar.SetMagic(currentMagic);
            }
        }
    }

    IEnumerator RestoreMagic() {
        isRestoring = true;
        float restoreRate = (.25f * magicLevel) / 1.5f; //was divided by 2

		for (float current = currentMagic; current <= maxMagic; current += restoreRate){
            currentMagic = current;
			magicBar.SetMagic(current);
			yield return new WaitForSeconds (Time.deltaTime);
		}

		currentMagic = maxMagic;	
        isRestoring = false;		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Flora"))
        {
            CollectFlora();
            magicBar.SetMagicLevel(magicLevel);

            Destroy(collision.gameObject);
        } 

    }

    IEnumerator CastCounter() {  //auto begins health point respawn after avoiding harm after x time
        isCounting = true;

        // Debug.Log("magic counter started");	
        yield return new WaitForSeconds(5);

        // Debug.Log("magic counter");	
        isCounting = false;	

        StartCoroutine ("RestoreMagic");
	}

    public void CancelCoroutines() {  
        if(isRestoring) {  //stop if restoring
            StopCoroutine("RestoreMagic");
            isRestoring = false;
        }

        if(isCounting) {  //stop if counting
            StopCoroutine("CastCounter");
            isCounting = false;
        }
    }
}
