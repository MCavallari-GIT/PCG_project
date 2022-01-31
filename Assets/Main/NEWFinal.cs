using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NEWFinal : MonoBehaviour
{
    public int width,height,length;
    int[, ,] map;
    int[, ,]second_map;
    public bool useRandomSeed;
    public string seed;
    public GameObject Tile_sand;
    public GameObject Tile_sea;
    public GameObject Tile_green_land;
    public int number_of_iteration;
    int iteration=0;

    void Start(){
        map=new int[height,length,width];
        second_map=map;
        //fill map with random values

        if (useRandomSeed) {
			//seed = "mimmo";//Time.time.ToString();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new System.Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            seed = new string(stringChars);

		}
        
		System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        for(int y=0;y<height;y++){
            for(int x=0;x<length;x++){
                for(int z=0;z<width;z++){
                    if(y==0){
                        map[y,x,z]=1;
                    }else if(x==length-1 || x==0 || z==0 || z==width-1){
                        map[y,x,z]=0;
                    }else {
                        if(pseudoRandom.Next(0,100)>=1 && pseudoRandom.Next(0,100)<=33){
                            map[y,x,z]=0;
                        } else if (pseudoRandom.Next(0,100)>33 && pseudoRandom.Next(0,100)<=66) {
                            map[y,x,z]=1;
                        } else{
                            map[y,x,z]=2;
                        }
                    }
                }
            }
        }

       for (int i = 0; i < number_of_iteration; i ++) {
			SmoothMap();
			iteration++;
	   }

       Draw();
     }//end Start method


    void SmoothMap(){
        int c=0;
        for(int y=1;y<height;y++){
            for(int x=0;x<length;x++){
                for(int z=0;z<width;z++){
                    int surrounding_lands=GetSurroundingLandCount(y,x,z);
                    if (surrounding_lands > 14 && surrounding_lands <21){
					    if(iteration%2==0){
						    second_map[y,x,z]=1;
					    } else{
						    map[y,x,z] = 1;
					    }
                        c++;
                    } else if ( surrounding_lands <= 13){
					    if(iteration%2==0){
						    second_map[y,x,z]=0;
					    } else{
						    map[y,x,z] = 0;
					    }
                        c++;
				    } else if(surrounding_lands >= 25){
                        if(iteration%2==0){
						    second_map[y,x,z]=2;
					    } else{
						    map[y,x,z] = 2;
					    }
                    }
                   
				}
            }
        }  
    }

    int GetSurroundingLandCount(int gridY,int gridX,int gridZ){
        int landCount=0;
        for(int y=gridY-1;y<=gridY;y++){
            for(int x=gridX-1;x<=gridX+1;x++){
                for(int z=gridZ-1;z<=gridZ+1;z++){
                    if (y>=0 && y<height && x>=0 && x<length && z>= 0 &&  z< width) {
                        if(iteration%2==0){
						landCount += map[y,x,z];
					    } else{
						    landCount += second_map[y,x,z];
					    }
                    } 
                }
            }
        }
        return landCount;
    }


  void Draw(){
		if (map != null && second_map!=null) {
            for(int y=0;y<height;y++){
                for (int x = 0; x < length; x ++) {
				    for (int z = 0; z < width; z ++) {
                        if(iteration%2!=0){
                            //Debug.Log("second");
                            if(second_map[y,x,z]==1){
                                Instantiate(Tile_sand, new Vector3(x,y,z), Quaternion.identity);
                            } else  if(second_map[y,x,z]==0) {
                               if(y<(height/2)+1){
                                    Instantiate(Tile_sea, new Vector3(x,y,z), Quaternion.identity);
                               }
                            }	else if(second_map[y,x,z]==2){
                                Instantiate(Tile_green_land, new Vector3(x,y,z), Quaternion.identity);
                            }
                        } else{
                            //Debug.Log("map");
                            if(map[y,x,z]==1){
                                Instantiate(Tile_sand, new Vector3(x,y,z), Quaternion.identity);
                            } else if(map[y,x,z]==0){
                                if(y<(height/2)+1){
                                    Instantiate(Tile_sea, new Vector3(x,y,z), Quaternion.identity);
                                }
                            }	else if(map[y,x,z]==2){
                                Instantiate(Tile_green_land, new Vector3(x,y,z), Quaternion.identity);
                            }
                        }	
				    }
			    }
            }
		}
  }
}


/*
better code but it seemed to me that the copy on result of map or second_map slowed the project of a lot 
(>60sec in comparison with the <15sec of the code currently in use)

   void Draw(){
		if (map != null && second_map!=null) {
            for(int y=0;y<height;y++){
                for (int x = 0; x < length; x ++) {
				    for (int z = 0; z < width; z ++) {
                        int [,,] result=new int[height,length,width];
                        if(iteration%2!=0){
                            result=second_map;
                        } else{
                            result=map;
                        }

                        if(result[y,x,z]==1){
                                Instantiate(Tile_sand, new Vector3(x,y,z), Quaternion.identity);
                        } else  if(result[y,x,z]==0) {
                            if(y<(height/2)+1){
                                Instantiate(Tile_sea, new Vector3(x,y,z), Quaternion.identity);
                            }
                        } else if(result[y,x,z]==2){
                                Instantiate(Tile_green_land, new Vector3(x,y,z), Quaternion.identity);
                        }
				    }
			    }
            }
		}
   }
*/
