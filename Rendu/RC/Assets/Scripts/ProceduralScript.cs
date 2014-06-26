using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ProceduralScript : MonoBehaviour {

    private int UID;

    [SerializeField]
    private List<Transform> blocks;

    //[SerializeField]
    //private float probability_off_room = 0.5f;

    [SerializeField]
    private List<objectForGenerationScript> objects;

    [SerializeField]
    private int max_random = 20;

    [SerializeField]
    private float probability_personnal_object = 0.5f;

    private string[,] grid;
    private int number_column;
    private int number_row;
    private int surface_availible;

    private Dictionary<string, objectForGenerationScript> dict_objects = new Dictionary<string, objectForGenerationScript>();
    private Dictionary<string, Transform> dict_block = new Dictionary<string,Transform>();
    private Dictionary<int, Dictionary<int, List<string>>> association = new Dictionary<int, Dictionary<int, List<string>>>();

	// Use this for initialization
	void Start () {
        if(Network.isServer)
        {
            StaticVariableScript settings = gameObject.GetComponent<StaticVariableScript>();
            UID = -1;
            //init surface and mapping
            number_column = (int)(gameObject.renderer.bounds.size.x);
            number_row = (int)(gameObject.renderer.bounds.size.z);
            surface_availible = number_column * number_row;
            grid = new string[number_column, number_row];
            //Main
            createDictionnary();
            clearGrid();
            create_interior();
            add_object_on_Room();
            addDoors();
            //build without door for baking
            build_plane(false);
            #if UNITY_EDITOR
            NavMeshBuilder.BuildNavMesh();
            #endif
            //build only door
            //build_plane(true);
            if (settings.GameMode == "escape")
                new SpawnKeyScript().generateKey();
        }
	}

    private void add_object_on_Room()
    {
        int[] space = count_space();
        foreach (string key in dict_objects.Keys)
        {
            for (int i = 1; i < UID; i++)
            {
                if (space[i] >= dict_objects[key].surface())
                {
                    if(check_space(key,i))
                    {
                        if (probability_personnal_object * dict_objects[key].Probability < Random.value)
                        {
                            int[] position = first_occurence_of(i);
                            create_object(position[0], position[1], key);
                        }
                    }
                }
            }
        }
    }

    private int[] first_occurence_of(int UID)
    {
        int[] position = new int[2];
        int max_X = 0;
        int max_Y = 0;
        for (int i = 1; i < number_column; i++)
        {
            for (int j = 1; j < number_row; j++)
            {
                if(grid[i,j] == UID.ToString())
                {
                    while(grid[i + max_X,j] == UID.ToString())
                    {
                        max_X++;
                    }   
                    while(grid[i,j + max_Y] == UID.ToString())
                    {
                        max_Y++;
                    }
                    position[0] = i + max_X/2;
                    position[1] = j + max_Y/2;
                    return position;
                }
            }
        }
        return position;
    }

    private int[] count_space()
    {
        int[] count_case_index = new int[UID];
        for (int index = 1; index < UID; index++)
        {
            count_case_index[index] = 0;
            for (int i = 1; i < number_column; i++)
            {
                for (int j = 1; j < number_row; j++)
                {
                    if (grid[i, j] == index.ToString())
                        count_case_index[index]++;
                }
            }
        }
        return count_case_index;
    }

    private bool check_space(string key,int UID)
    {
        int width = dict_objects[key].sizeX + 6;
        int height = dict_objects[key].sizeZ + 6;
        for (int i = 1; i < number_column; i++)
        {
            for (int j = 1; j < number_row; j++)
            {
                if (grid[i, j] == UID.ToString())
                {
                    int test_y = 0;
                    int test_x = 0;
                    while (test_y < height)
                    {
                        while (test_x < width)
                        {
                            if (grid[i + test_x, j + test_y] != UID.ToString())
                                return false;
                            test_x++;
                        }
                        test_y++;
                    }
                    return true;
                }
            }
        }
        return false;
    }

    private void printGridForDebug()
    {
        string map = "";
        for(int i = 0;i < number_column;i++)
        {
            map += ";\n";
            for(int j = 0; j < number_row;j++)
            {
                map += grid[i, j].ToString() + ",";
            }
        }
        Debug.Log(map);
    }

    private void createDictionnary()
    {
        for (int i = 0; i < blocks.Count;i++ )
            dict_block.Add(blocks[i].name, blocks[i]);
        for (int i = 0; i < objects.Count; i++)
            dict_objects.Add(objects[i].visual.name, objects[i]);
    }

    private void clearGrid()
    {
        create_room(0, 0,number_column,number_row);
        changeAllOccur("0", "vide");
    }

    private void create_interior()
    {
        float probabilityValue = Random.value;
        //string object_choose = "";
        for (int i = 1; i < number_column; i++)
        {
            for (int j = 1; j < number_row; j++)
            {
                prepare_room(i, j);
            }
        }
    }

    private void prepare_room(int i,int j)
    {
        int width = Random.Range(3, max_random);
        int height = Random.Range(3, max_random);
        if (grid[i, j] == "vide")
        {
            if (((i + width) > number_column))
                width = number_column - 1 - i;
            if (((j + height) > number_row))
                height = number_row - 1 - j;
            //surface = width * height;
            if (((i + width) < number_column) && ((j + height) < number_row) && width > 2 && height > 2)
            {
                create_room(i - 1, j - 1, width + 1, height + 1);
                //surface_availible -= surface;
            }
        }
    }

    private void create_object(int startX, int startY, string key)
    {
        int width = dict_objects[key].sizeX;
        int height = dict_objects[key].sizeZ;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (j == 0 && i == 0)
                    grid[startX + i, startY + j] = key;
                else
                    grid[startX + i, startY + j] = "occupee";
            }
        }
    }

    private void create_room(int startX,int startY,int width,int height)//start haut gauche
    {
        UID++;
        for(int i =0;i < width;i++)
        {
            for(int j=0;j < height;j++)
            {
                if (j == 0 && i == 0)
                    grid[startX + i, startY + j] = "Angle_Left_Bottom";
                else if (j == height - 1 && i == 0)
                    grid[startX + i, startY + j] = "Angle_Left_Top";
                else if (j == height - 1 && i == width - 1)
                    grid[startX + i, startY + j] = "Angle_Right_Top";
                else if (j == 0 && i == width - 1)
                    grid[startX + i, startY + j] = "Angle_Right_Bottom";
                else if (j == 0)
                    grid[startX + i, startY + j] = "Horizontal_bas";
                else if (j == height - 1)
                    grid[startX + i, startY + j] = "Horizontal_haut";
                else if (i == 0)
                    grid[startX + i, startY + j] = "Vertical_haut";
                else if (i == width - 1)
                    grid[startX + i, startY + j] = "Vertical_bas";
                else
                    grid[startX + i, startY + j] = UID.ToString();
            }
        }
    }

    //Ici on check les UID des salles
    //Deux salles différentes on un UID différent 
    //donc sur deux UID différent ont peu ouvrir une porte
    private void addDoors()
    {
        //On change tout les vides en un UID pour ouvrir les portes vers eux aussi
        changeAllOccur("vide", UID++.ToString());
        createAssociation();
        int tmp;
        int more;
        int less;
        //Pour savoir si une porte doit être ouverte on check maximum +1 ou -1 case pour trouver deux UID different
        //C'est pourquoi les boucles commence a 1 et -1 (évite une redondance de test)
        for (int i = 1; i < number_column -1; i++)
        {
            for (int j = 1; j < number_row -1; j++)
            {
                if(!int.TryParse(grid[i, j],out tmp))
                {
                    if(int.TryParse(grid[i+1, j],out more) && int.TryParse(grid[i-1, j],out less) && !association[more][less].Contains("V"))
                    {
                        grid[i, j] = "porte_vertical";
                        association[more][less].Add("V");
                        association[less][more].Add("V");
                    }
                    if (int.TryParse(grid[i, j+1], out more) && int.TryParse(grid[i,j-1], out less) &&  !association[more][less].Contains("H"))
                    {
                        grid[i, j] = "porte_horizontal";
                        association[more][less].Add("H");
                        association[less][more].Add("H");
                    }
                }
            }
        }
    }

    private void createAssociation()
    {
        for(int i = 1;i < UID;i++)
        {
            association.Add(i,new Dictionary<int,List<string>>());
            for (int j = 1; j < UID; j++)
            {
                association[i].Add(j, new List<string>(2));
                if (i == j)
                {
                    association[i][j].Add("V");
                    association[i][j].Add("H");
                }
            }
        }
    }

    private void changeAllOccur(string search,string replace)
    {
        for (int i = 0; i < number_column; i++)
        {
            for (int j = 0; j < number_row; j++)
            {
                if (grid[i,j] == search)
                    grid[i, j] = replace;
            }
        }
    }

    private void build_plane(bool _builddoor)
    {
        string door_type;
        int res;
        float posX = 0;
        float posZ = 0;
        Vector3 pos;

        for (int i = 0; i < number_column; i++)
        {
            for (int j = 0; j < number_row; j++)
            {
                if (!_builddoor)
                {
                    if (!int.TryParse(grid[i, j], out res) && grid[i, j] != "porte_vertical" && grid[i, j] != "porte_horizontal" &&  grid[i,j] != "occupee")
                    {
                        posX = i - (number_column / 2) + 0.5f;
                        posZ = j - (number_row / 2) + 0.5f;
                        pos = new Vector3(posX, 1, posZ);
                        if(!dict_objects.ContainsKey(grid[i,j]))
                            Network.Instantiate(dict_block[grid[i, j]], pos, Quaternion.identity,0);
                        else
                            Network.Instantiate(dict_objects[grid[i, j]].visual, pos, Quaternion.identity, 0);
                    }
                }
                else 
                {
                    //nous devons gérer le cas des portes "hautes" et "basses" pour s'aligner avec les murs
                    if(grid[i, j] == "porte_vertical")
                    {
                        if (grid[i, j - 1] == "Vertical_haut" || grid[i, j + 1] == "Vertical_haut" || grid[i, j - 1].Contains("Top") )
                            door_type = "_haut";
                        else
                            door_type = "_bas";
                        posX = i - (number_column / 2) + 0.5f;
                        posZ = j - (number_row / 2) + 0.5f;
                        pos = new Vector3(posX, 1f, posZ);
                        Network.Instantiate(dict_block[grid[i, j] + door_type], pos, Quaternion.identity,0);
                    }else if(grid[i, j] == "porte_horizontal"){
                        if (grid[i - 1, j] == "Horizontal_haut" || grid[i + 1, j] == "Horizontal_haut" || grid[i-1, j].Contains("Top") )
                            door_type = "_haut";
                        else
                            door_type = "_bas";
                        posX = i - (number_column / 2) + 0.5f;
                        posZ = j - (number_row / 2) + 0.5f;
                        pos = new Vector3(posX, 1f, posZ);
                        Network.Instantiate(dict_block[grid[i, j] + door_type], pos, Quaternion.identity,0);
                    }
                }
            }
        }
    }

}
