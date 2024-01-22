using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    
    [SerializeField] private float playerSpeed;
    private Animator PlrAnim;
    [SerializeField] public List<Transform> papers { get; private set; } = new List<Transform>();
    [SerializeField] private Transform paperPlace;
    private float YAxis, delay;
    public TextMeshProUGUI MoneyCounter;
    public static PlayerManager PlayerManagerInstance;
    void Start()
    {
        
        PlrAnim = GetComponent<Animator>();
        
        papers.Add(paperPlace);

        PlayerManagerInstance = this;
    }

    // Update is called once per frame
    void Update()
    { 
      

        if (papers.Count > 1)
        {
            for (int i = 1; i < papers.Count; i++)
            {
                var firstPaper = papers.ElementAt(i - 1);
                var secondPaper = papers.ElementAt(i);
                
                secondPaper.position = new Vector3(Mathf.Lerp(secondPaper.position.x,firstPaper.position.x,Time.deltaTime * 15f),
                Mathf.Lerp(secondPaper.position.y,firstPaper.position.y + 0.17f,Time.deltaTime * 15f),firstPaper.position.z);
            }
        }

        if (Physics.Raycast(transform.position,transform.forward,out var hit,1f))
        {
            Debug.DrawRay(transform.position,transform.forward * 1f,Color.green);
            
            if (hit.collider.CompareTag("table") && papers.Count < 21)
            {
                
                if (hit.collider.transform.childCount > 2)
                {
                    
                    var paper = hit.collider.transform.GetChild(1);
                    paper.rotation = Quaternion.Euler(paper.rotation.x,Random.Range(0f,180f),paper.rotation.z);
                    papers.Add(paper);
                    paper.parent = null;

                    if (hit.collider.transform.parent.GetComponent<Printer>().CountPapers > 1)
                        hit.collider.transform.parent.GetComponent<Printer>().CountPapers--;

                    if (hit.collider.transform.parent.GetComponent<Printer>().YAxis > 0f)
                        hit.collider.transform.parent.GetComponent<Printer>().YAxis -= 0.17f;

                    PlrAnim.SetBool("carry",true);
                    PlrAnim.SetBool("run",false);
                }
            }

            if (hit.collider.CompareTag("pp") && papers.Count > 1)
            {
                var WorkDesk = hit.collider.transform;
                hit.transform.gameObject.GetComponent<workDesk>().Work();

                if (WorkDesk.childCount > 0)
                {
                    YAxis = WorkDesk.GetChild(WorkDesk.childCount - 1).position.y;
                }
                else
                {
                    YAxis = WorkDesk.position.y;
                }

                for (var index = papers.Count - 1; index >= 1; index--)
                {
                    papers[index].DOJump(new Vector3(WorkDesk.position.x, YAxis, WorkDesk.position.z), 2f, 1, 0.2f)
                        .SetDelay(delay).SetEase(Ease.Flash);

                    papers.ElementAt(index).parent = WorkDesk;
                    papers.RemoveAt(index);

                    YAxis += 0.17f;
                    delay += 0.02f;
                }

                if (papers.Count <= 1)
                {
                    
                    PlrAnim.SetBool("idle",true);
                    PlrAnim.SetBool("RunWithPapers",false);
                }
                
            }
        }
        else
        {
            Debug.DrawRay(transform.position,transform.forward * 1f,Color.red);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("dollar"))
        {
            Destroy(other.gameObject);
            
            PlayerPrefs.SetInt("dollar",PlayerPrefs.GetInt("dollar") + 5);

            MoneyCounter.text = "$" + PlayerPrefs.GetInt("dollar").ToString();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("pp"))
        {
           
            PlrAnim.SetBool("RunWithPapers",false);
            PlrAnim.SetBool("idle",false);
            PlrAnim.SetBool("run",true);
            delay = 0f;
        }
        
        if (other.CompareTag("table"))
        {
            if (papers.Count > 1)
            {
                PlrAnim.SetBool("carry",false);
                PlrAnim.SetBool("RunWithPapers",true);
            }
            else
            {
                PlrAnim.SetBool("run",true);
            }
        }
    }
}
