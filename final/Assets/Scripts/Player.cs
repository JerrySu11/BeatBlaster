using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Camera m_cam;
    public UnityEvent<int> OnHealthChange;
    public GameObject arrow;
    //Player health
    public int maxHealth = 4;
    public int health = 4;
    public Slider healthBar;
    private int damage;
    //Teleport distance
    private float moveDist = 1.0f;
    private bool superDash = false;
    //default ray attack distance
    public float defaultLength = 6.0f;
    private string state = "select";
    private bool invincible = false;

    public AudioSource hitSound;

    public GameController game;

    private LineRenderer m_LineRenderer = null;

    public GameObject dash1;
    public GameObject dash2;
    public GameObject dash3;

    public class MyIntEvent : UnityEvent<int>
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
        arrow.SetActive(false);

        Color original = dash1.GetComponent<MeshRenderer>().material.color;
        dash1.transform.SetParent(null, true);
        dash2.transform.SetParent(null, true);
        dash3.transform.SetParent(null, true);
        dash1.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0f);
        dash2.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0f);
        dash3.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0f);

        PlayableDirector timeline = GetComponent<PlayableDirector>();
        timeline.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == "select")
        {         
            if (!arrow.activeInHierarchy)
            {
                arrow.SetActive(true);
            }
            
            Vector3 mousedir = mouseDir(transform);
            arrow.transform.position = transform.position + mousedir * defaultLength;
            arrow.transform.eulerAngles = new Vector3(0, 0, mousedir.x>=0.0f? Mathf.Rad2Deg * Mathf.Atan(mousedir.y / mousedir.x) + 180f : Mathf.Rad2Deg * Mathf.Atan(mousedir.y / mousedir.x));

            //Modify below to change trigger conditions for fire and teleport
            if (Input.GetKeyDown(KeyCode.Space))
            {

                fireRay();  
                state = "fire";
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                teleport();
                state = "teleport";
            }
        }

        else if (state == "teleport")
        {
            if (arrow.activeInHierarchy)
            {
                arrow.SetActive(false);
            }

        }
        else if (state == "fire")
        {
            if (arrow.activeInHierarchy)
            {
                arrow.SetActive(false);
            }

        }

    }

    private Vector3 mouseDir(Transform current)
    {
        Vector3 mousePos = m_cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mousedir = new Vector3(mousePos.x - current.transform.position.x, mousePos.y - current.transform.position.y, 0);
        return mousedir.normalized;
    }

    public int getHealth()
    {
        return health;
    }

    public void setHealth(int healthSet)
    {
        if (health < healthSet) {
            StartCoroutine("addHealthAnimation");
        }

        health = healthSet;

        healthBar.value = health;

        if (health <= 0)
        {
            game.GameDie();
        }
    }

    //Teleport player towards the mouse direction by moveDist
    public void teleport()
    {
        Vector3 mousedir = mouseDir(transform);
        //players can move farther if they are hit previously.
        float actualMovDist = superDash ? moveDist * 3f : moveDist;
        Vector3 targetPos = transform.position + mousedir * actualMovDist;
        StartCoroutine(teleportGradual(transform.position, targetPos));
        StartCoroutine(dashFade(transform.position, targetPos));
        if (superDash)
        {
            
            superDash = false;
        }
        
    }
    IEnumerator teleportGradual(Vector3 startPos,Vector3 endPos)
    {

        for (int i = 1; i <= 10; i++)
        {
            transform.position = Vector3.Lerp(startPos, endPos, 0.1f * i);
            yield return new WaitForSeconds(0.01f);
        }
        

    }

    IEnumerator dashFade(Vector3 startPos, Vector3 endPos)
    {
        
        yield return new WaitForSeconds(.02f);
        StartCoroutine(dashFade3(startPos, endPos));

        yield return new WaitForSeconds(.02f);
        StartCoroutine(dashFade2(startPos, endPos));

        yield return new WaitForSeconds(.02f);
        StartCoroutine(dashFade1(startPos, endPos));

    }
        //insert or modify code here to determine when user is back to "select" state
        //state = "select";
    

    IEnumerator dashFade1(Vector3 startPos, Vector3 endPos)
    {
        Color original = dash1.GetComponent<MeshRenderer>().material.color;
        dash1.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0.5f);

        dash1.transform.position = Vector3.Lerp(endPos, startPos, 0.4f);

        Vector3 movementVec = (endPos - startPos).normalized;
        dash1.transform.eulerAngles = new Vector3(0, 0, (movementVec.x >= 0.0f ? Mathf.Rad2Deg * Mathf.Atan(movementVec.y / movementVec.x) + 180f : Mathf.Rad2Deg * Mathf.Atan(movementVec.y / movementVec.x)) + 90);

        //Color current = dashAnimation.material.color;
        //dashAnimation.material.color = new Color(current.r, current.g, current.b, 0.5f);
        //dashAnimation.SetPosition(0, startPos);
        //dashAnimation.SetPosition(1, endPos);
        for (int i = 1; i <= 10; i++)
        {

            dash1.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0.5f - 0.05f * i);
            yield return new WaitForSeconds(.05f);
        }
        //insert or modify code here to determine when user is back to "select" state
        //state = "select";
    }

    IEnumerator dashFade2(Vector3 startPos, Vector3 endPos)
    {
        Color original = dash2.GetComponent<MeshRenderer>().material.color;
        dash2.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0.5f);

        dash2.transform.position = Vector3.Lerp(endPos, startPos, 0.6f);

        Vector3 movementVec = (endPos - startPos).normalized;
        dash2.transform.eulerAngles = new Vector3(0, 0, (movementVec.x >= 0.0f ? Mathf.Rad2Deg * Mathf.Atan(movementVec.y / movementVec.x) + 180f : Mathf.Rad2Deg * Mathf.Atan(movementVec.y / movementVec.x)) + 90);

        //Color current = dashAnimation.material.color;
        //dashAnimation.material.color = new Color(current.r, current.g, current.b, 0.5f);
        //dashAnimation.SetPosition(0, startPos);
        //dashAnimation.SetPosition(1, endPos);
        for (int i = 1; i <= 10; i++)
        {

            dash2.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0.5f - 0.05f * i);
            yield return new WaitForSeconds(.05f);
        }
        //insert or modify code here to determine when user is back to "select" state
        //state = "select";
    }

    IEnumerator dashFade3(Vector3 startPos, Vector3 endPos)
    {
        Color original = dash1.GetComponent<MeshRenderer>().material.color;
        dash3.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0.5f);

        dash3.transform.position = Vector3.Lerp(endPos, startPos, 0.8f);

        Vector3 movementVec = (endPos - startPos).normalized;
        dash3.transform.eulerAngles = new Vector3(0, 0, (movementVec.x >= 0.0f ? Mathf.Rad2Deg * Mathf.Atan(movementVec.y / movementVec.x) + 180f : Mathf.Rad2Deg * Mathf.Atan(movementVec.y / movementVec.x)) + 90);

        //Color current = dashAnimation.material.color;
        //dashAnimation.material.color = new Color(current.r, current.g, current.b, 0.5f);
        //dashAnimation.SetPosition(0, startPos);
        //dashAnimation.SetPosition(1, endPos);
        for (int i = 1; i <= 10; i++)
        {

            dash3.GetComponent<MeshRenderer>().material.color = new Color(original.r, original.g, original.b, 0.5f - 0.05f * i);
            yield return new WaitForSeconds(.05f);
        }
        //insert or modify code here to determine when user is back to "select" state
        //state = "select";
    }

    public void fireRay()
    {
        Vector3 mousedir = mouseDir(transform);
        RaycastHit[] hits;

        hits = Physics.RaycastAll(transform.position,mousedir, defaultLength);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].collider.gameObject.GetComponent<Enemy>().gotHit(50);
            if (hits[i].collider.gameObject.GetComponent<Enemy>().health == 0)
            {
                game.destroyEnemySound.Play(0);
                hits[i].collider.gameObject.GetComponent<Enemy>().die();
                StartCoroutine("delayDestroyEnemy", hits[i].collider.gameObject);
            }
        }
        
        //reset color
        Color current = m_LineRenderer.material.color;
        m_LineRenderer.material.color = new Color(current.r, current.g, current.b, 1f);
        m_LineRenderer.SetPosition(0, transform.position + mousedir * 0.5f);
        m_LineRenderer.SetPosition(1, transform.position + mousedir * defaultLength);
        
        
        StartCoroutine("fireRayFade");
    }
    IEnumerator delayDestroyEnemy(GameObject enemy)
    {
        yield return new WaitForSeconds(0.25f);
        game.DestroyEnemy(enemy);
        Destroy(enemy);
    }

    IEnumerator fireRayFade()
    {
        for (int i = 0; i < 10; i++)
        {
            Color current = m_LineRenderer.material.color;
            m_LineRenderer.material.color = new Color(current.r,current.g,current.b,current.a-0.1f);
            yield return new WaitForSeconds(.1f); 
        }
        //insert or modify code here to determine when user is back to "select" state
        //state = "select";
    }
    
    IEnumerator enemyHitFade()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.red;

        //invincible for a while
        invincible = true;
        //next dash is a superDash that teleports 3x the default distance
        superDash = true;
        for (int i = 0; i < 10; i++)
        {
            Color current = gameObject.GetComponent<Renderer>().material.color;
            gameObject.GetComponent<Renderer>().material.color = new Color(current.r, current.g+0.1f, current.b+0.1f, i%2==0?0f:1f);
            yield return new WaitForSeconds(.05f);
        }
        invincible = false;
    }

    IEnumerator addHealthAnimation()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.green;
        for (int i = 0; i < 10; i++) {
            Color current = gameObject.GetComponent<Renderer>().material.color;
            gameObject.GetComponent<Renderer>().material.color = new Color(current.r+0.1f, current.g, current.b+0.1f, 1f);
            yield return new WaitForSeconds(.1f);
        }
    }

    IEnumerator waitForSecond(float sec)
    {
        yield return new WaitForSeconds(sec);
        //insert or modify code here to determine when user is back to "select" state
        state = "select";
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy" && !invincible)
        {
            this.setHealth(this.getHealth() - 1);
            hitSound.Play(0);
            StartCoroutine("enemyHitFade");
        }
    }
}
