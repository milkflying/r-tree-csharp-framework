#include <iostream>
#include <math.h>
//#include <list>

using namespace std;

const float TOLERANCE = 1E-7;

float getRandFloat(float low, float high);
int getRandInt(int low, int high);


int main (int argc, char** argv)
{
	const int DEF_ITEM_COUNT = 1000;
	const int MAXDIMEN = 10;
	const float DEF_EXTENT = 0.0;
	const float DEF_LOWER = 0.0;
	const float DEF_UPPER = 1.0;
	const int DEF_DIMEN = 2;
	const int DEF_SEED = 0;
	const float DEF_QR = 0.5;
	const int DEF_CUTOFF = 1000;
	const int DEF_KNN = 64;
	const float DEF_REGION = 0.05;
	const int DEF_HIST = 1000;

	

	int item_count = DEF_ITEM_COUNT;
	int dimen = DEF_DIMEN;				//currently ignored
	int seed = DEF_SEED;
	float extent = DEF_EXTENT;
	float min_bound = DEF_LOWER;
	float max_bound = DEF_UPPER;
	float query_ratio = DEF_QR;
	int cutoff = DEF_CUTOFF;
	float qregion_min = DEF_REGION;
	float qregion_max = DEF_REGION;
	int qknn_max = DEF_KNN;
	int hist = DEF_HIST;



	if ((argc == 2) && (argv[1][0] == '?'))
	{
		cout << "Workload Generator -n number of workload items (1000)\n"
			 << "-d dimen (MAXDIMEN=10, 2) \n"
			 << "-l lower (0) \n"
			 << "-u upper (1) \n"
			 << "-e new object extent (0,point) \n"
			 << "-s random seed (0) \n"
			 << "-h query result history (1000) \n"
			 << "-q fraction of items that are queries (0.5) \n"
			 << "-k max k for kNN (64)\n"
			 << "-a min radius,side for region query (0.05) \n"
			 << "-b max radius,side for region query (0.05) \n"
			 << "-c existing data record cutoff (1000) \n";
		exit(0);
	}


	for (int a=1; a<argc; a++)
	{
		if (argv[a][0] == '-')
		{
			switch (argv[a][1])
			{
			case 'd':
				dimen = (int)atol(argv[++a]);
				break;
			case 's':
				seed = (int)atol(argv[++a]);
				break;
			case 'k':
				qknn_max = (int)atol(argv[++a]);
				break;
			case 'c':
				cutoff = (int)atol(argv[++a]);
				break;
			case 'n':
				item_count = (int)atol(argv[++a]);
				break;
			case 'h':
				hist = (int)atol(argv[++a]);
				break;
			case 'l':
				min_bound = atof(argv[++a]);
				break;
			case 'u':
				max_bound = atof(argv[++a]);
				break;
			case 'e':
				extent = atof(argv[++a]);
				break;
			case 'q':
				query_ratio = atof(argv[++a]);
				break;
			case 'a':
				qregion_min = atof(argv[++a]);
				break;
			case 'b':
				qregion_max = atof(argv[++a]);
				break;
			}
		}
		else
		{
			fprintf(stderr,"invalid flag (%s)\n",argv[a]);
			exit(0);
		}
	}

	srand(seed);
	
	int next_id = cutoff+1;
	//list<int> removed;
	//list<int>::iterator iter;
	//iter = removed.begin();
	

	for (int i=1; i<=item_count; i++)
	{
		float op_choice = getRandFloat(0.0,1.0);
		if (op_choice < query_ratio)
		{
			//Execute query
			int q_choice = getRandInt(0,2);
			float loc[2] = {getRandFloat(min_bound+qregion_max, max_bound-qregion_max), getRandFloat(min_bound+qregion_max, max_bound-qregion_max)};
			switch (q_choice)
			{
			case 0:
			{
				//Execute range query			
				float radius = getRandFloat(qregion_min, qregion_max);
				cout << i << ",Q,R," << loc[0] << "," << loc[1] << "," << radius << endl;

				break;
			}
			case 1:
			{
				//Execute window query
				float side = getRandFloat(qregion_min, qregion_max);
				cout << i << ",Q,W," << loc[0] << "," << loc[1] << "," << side << endl;
				break;
			}
			case 2:
			{
				//Execute kNN query
				int k = getRandInt(1, qknn_max);
				cout << i << ",Q,K," << loc[0] << "," << loc[1] << "," << k << endl;
				break;
			}
			default:
			{
				//Should never happen
				cout << "QCHOICE ERROR!" << endl;
				break;
			}
			}
		}
		else
		{
			//Execute update operation
			int u_choice = getRandInt(0,2);
			switch (u_choice)
			{
			case 0:
			{
				//Execute insert			
				float loc[2] = {getRandFloat(min_bound+extent, max_bound-extent), getRandFloat(min_bound+extent, max_bound-extent)};
				cout << i << ",I," << next_id++ << "," << loc[0] << "," << loc[1] << "," << extent << endl;

				break;
			}
			case 1:
			{
				//Execute update
				float loc[2] = {getRandFloat(min_bound+extent, max_bound-extent), getRandFloat(min_bound+extent, max_bound-extent)};
				int cur_id = getRandInt(1,hist);
				cout << i << ",U," << cur_id << "," << loc[0] << "," << loc[1] << "," << extent << endl;
				break;
			}
			case 2:
			{
				//Execute deletion

				int cur_id = getRandInt(1,hist);
				cout << i << ",D," << cur_id << endl;


				// OLD CODE THAT ASSUMED WE COULD USE RECORD ID
				// (bad idea)
				/*
				int cur_id = getRandInt(1, next_id-1);
				int count = 0;
				int tmp_id = cur_id;
				bool in_list = true;
				while ((cnt<next_id)&&(in_list==true))
				{
					in_list = false;
					iter = removed.begin();
					while (!(in_list)&&(iter!=removed.end()))
					{
						if (*iter==tmp_id)
							in_list = true;
						iter++;
					}
					cnt++;
					tmp_id = (tmp_id+1==next_id) ? 1 : tmp_id+1;
				}

				if (in_list)
					cout << "ERROR: CANNOT DELETE (tree may be empty)!\n";
				else
					cout << i << ",D," << tmp_id << "," << loc[1] << "," << k << endl;
				*/

				break;
			}
			default:
			{
				//Should never happen
				cout << "QCHOICE ERROR!" << endl;
				break;
			}
			}

		}
	}



	return 0;
}

// generates a float between low and high (inclusive)
float getRandFloat(float low, float high)
{
	float temp;

	// check ordering of inputs
	if (low > high)
	{
		temp = low;
		low = high;
		high = temp;
	}

	//check if inputs are the same
	if (fabs(low - high) < TOLERANCE)
		return low;

	// generate number in specified range
	temp = (rand() / ((float)(RAND_MAX) + 1.0)) * (high - low) + low;
	return temp;
}

// generates an integer between low and high (inclusive)
int getRandInt(int low, int high)
{
	int temp;

	// check ordering of inputs
	if (low > high)
	{
		temp = low;
		low = high;
		high = temp;
	}

	//check if inputs are the same
	if ((low - high) == 0)
		return low;

	// generate number in specified range
	temp = low + int((high-low+1)*rand()/(RAND_MAX + 1.0));
	return temp;
}
