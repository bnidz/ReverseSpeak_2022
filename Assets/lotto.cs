using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lotto : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        check();

    }

    void check()
    {
       
        _results = Lotto();
        if (!CheckNums(_results))
        {
            Start();
        }
        else
            getLis();

        //results(_results);

    }

    void getLis()
    {

        lis = Random.Range(1, 40);
        if (!CheckLis(lis))
        {
            getLis();
        }
        else
            results(_results);
    }

    public int[] Lotto()
    {
        int[] numbers = {1,1,1,1,1,1,1};

        for (int i = 0; i < 7; i++)
        {
            numbers[i] = Random.Range(1, 40);

            //for (int y = 0; y < 7; y++)
            //{
            //    if (numbers[i] == numbers[y])
            //    {
            //        numbers[i] = getnumber(numbers[y]);

            //    }
            //}
        
        }
        return numbers;
        //CheckNums(numbers);
        //Debug.Log(numbers[0].ToString() +" "+ numbers[1].ToString() + " " + numbers[2].ToString() + " " + numbers[3].ToString() + " " + numbers[4].ToString() + " " + numbers[5].ToString() + " " + numbers[6].ToString());

    }

    public int getnumber(int change)
    {

        int return_no = Random.Range(1, 40);

        while(change == return_no)
        {
            return_no = Random.Range(1, 40);

        }

        return return_no;
    }

    bool CheckLis(int lis)
    {

        for (int i = 0; i < 7; i++)
        {
            if (lis == _results[i])
                return false;
        }

        return true;
    }


    public int[] _results;
    bool CheckNums(int[] nums)
    {

        for (int i = 0; i < 7; i++)
        {
            for (int y = 0; y < 7; y++)
            {
                if(nums[i] == nums[y] && y != i)
                {
                    return false;
                }
            }
        }

        _results = nums;
        return true;
    }
    int lis;
    void results(int[] numbers)
    {
        Debug.Log(numbers[0].ToString() + " " + numbers[1].ToString() + " " + numbers[2].ToString() + " " + numbers[3].ToString() + " " + numbers[4].ToString() + " " + numbers[5].ToString() + " " + numbers[6].ToString());
        Debug.Log("Lisänum. " + lis);

    }
}
