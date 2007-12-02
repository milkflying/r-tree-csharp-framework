#include <stdio.h>
#include <math.h>
#include <stdlib.h>
#include <string.h>


#define SEED    23121972
#define MAXDIMEN	10
#define MAXCLUS		10
#define LOWERBOUND	0
#define UPPERBOUND	1000
#define EXTENT		1


void swap(float& l,float& h)
{
       float t = l;
       l = h;
       h = t;
}


float uniform_gen(const float min,const float max,
				  const float ext)
{
	float r;
	float wid = (max-min);
	do
	{
		r = (float)((rand() / (RAND_MAX+1.0)) * wid);
	} while ((r-ext/2 < min) || (max < r+ext/2));
	return r;
}


/*
void uniform_gen(const float xl,const float xh,
				 const float yl,const float yh,
				 const float len,const float wid,
				 const float xe,const float ye,
				 float* px,float* py)
{
       float widx = (xh - xl);
       float widy = (yh - yl);


       do
       {
               do
               {
                       *px = (float)((rand()/(RAND_MAX+1.0)) * widx);
                       *py = (float)((rand()/(RAND_MAX+1.0)) * widy);
               } while ((*px < xl || *px+len > xh) ||
                       (*py < yl || *py+wid > yh));
       } while ((*px <= xe && xe <= *px+len) &&
               (*py <= ye && ye <= *py+wid));


       return;
}
*/


float gaussian_gen(const float min, const float max,
				   const float mean, const float sigma,const float ext)
{
       //const float mean = (min + max)/2;
       float v1, v2;
       float s,x;


       do{
               do {
                       v1 = (float)((rand() - (RAND_MAX/2))/(RAND_MAX+1.0));
                       v2 = (float)((rand() - (RAND_MAX/2))/(RAND_MAX+1.0));
                       s = v1*v1 + v2*v2;
               } while (s >= 1.);
               x = (float)(v1 * sqrt ( -2. * log (s) / s));
               /*  x is normally distributed with mean and sigma.  */
               x = x * sigma + mean;
       } while ((x-ext/2 < min) || (max < x+ext/2));
       return x;
}


/*
void gaussian_gen(
               const float xl,const float xh,
               const float yl,const float yh,
               const float xs,const float ys,
               const float len,const float wid,
               const float xe,const float ye,
               float* px,float* py)
{
       do
       {
               do
               {
                       (*px) = gaussian(xl,xh,xs);
                       (*py) = gaussian(yl,yh,ys);


               } while ((*px < xl || *px+len > xh) ||
                       (*py < yl || *py+wid > yh));
       } while ((*px <= xe && xe <= *px+len) &&
               (*py <= ye && ye <= *py+wid));


       return;
}
*/


/* usage: gen -n number of data items
             -a max. data space area (x,y) default: (1000.0,1000.0)
                         -d object size (length,width) default: (0.0,0.0)
                         -s random seed default: 1997
                         -t type of distributions (Uniform/Gaussian)
                         -v variance default: (100,100)
                         -e exclusive location (x,y) where no object can
                reside in/touch this (500,500)
*/
int main(int argc,char** argv)
{
	int num=-1;
	int seed=SEED;
	int dimen=2;
	float low[MAXDIMEN];
	float up[MAXDIMEN];
	float ext[MAXDIMEN];
	float exc[MAXDIMEN];
	float mean[MAXDIMEN];
	float var[MAXDIMEN];
	float clow[MAXDIMEN][MAXCLUS];
	float cup[MAXDIMEN][MAXCLUS];

	bool point=false;
	bool query=false;
	char type = 'U';
	int numcluster=1;

	if (argc == 1)
	{
		fprintf(stderr,
			"gen -n number of data items \n"
			"-d dimen (MAXDIMEN=10) \n"
			"-p point flag \n"
			"-l lower ($d) \n"
			"-u upper (%d) \n"
			"-e object extent default: (%d) \n"
			"-s random seed default: (%d) \n"
			"-t type of distribution: (U)niform or (G)aussian \n"
			"-c #clusters\n"
			"-m mean default: (only when -t G is specified) \n"
			"-v variance default: (only when -t G is specified) \n"
			"-x exclusive location where no object can reside in/touch default:(%f,%f). \n\n",
			LOWERBOUND, UPPERBOUND, EXTENT, SEED);
		exit(0);
	}


	for (int a=1; a<argc; a++)
	{
		if (argv[a][0] == '-')
		{
			switch (argv[a][1])
			{
			case 'c':
				numcluster = atol(argv[++a]);
				break;
			case 'd':	// dimension
				dimen = atol(argv[++a]);
				break;
			case 'e':
				{
					ext[0] = (float)atof(argv[++a]);
					for (int d=1; d<dimen; d++)
						ext[d] = ext[0];
				}
				break;
			case 'l':
				{
					low[0] = (float)atof(argv[++a]);
					for (int d=1; d<dimen; d++)
						low[d] = low[0];
				}
				break;
			case 'u':
				{
					up[0] = (float)atof(argv[++a]);
					for (int d=1; d<dimen; d++)
						up[d] = up[0];
				}
				break;
			case 'n':
				num = (int)atol(argv[++a]);
				break;
			case 'p':
				point = true;
				break;
			case 'q':
				query = true;
				break;
			case 's':
				seed = (int)atol(argv[++a]);
				srand(seed);
				break;
			case 't':
				type = argv[++a][0];
				break;
			case 'm':
				{
					mean[0] = (float)atof(argv[++a]);
					for (int d=1; d<dimen; d++)
						mean[d] = mean[0];
				}
				break;
			case 'v':
				{
					var[0] = (float)atof(argv[++a]);
					for (int d=1; d<dimen; d++)
						var[d] = var[0];
				}
				break;
			case 'x':
				{
					exc[0] = (float)atof(argv[++a]);
					for (int d=1; d<dimen; d++)
						exc[d] = exc[0];
				}
			}
		}
		else
		{
			fprintf(stderr,"invalid flag (%s)\n",argv[a]);
			exit(0);
		}
	}

	if (type == 'c' || type == 'C')
	{
		for (int d=0; d<dimen; d++)
		{
			clow[d][0] = 0;
			cup[d][numcluster-1] = up[d];
			float w = up[d] / numcluster;
			for (int i=0; i<numcluster-1; i++)
				clow[d][i+1] = cup[d][i] = clow[d][i] + w;
		}	
	}

	
	float r[MAXDIMEN];
	float rl[MAXDIMEN];
	float ru[MAXDIMEN];
	if (query) fprintf(stdout,"%d\n",num);
	for (int i=0; i<num; i++)
	{
		bool valid=false;
		while (!valid)
		{
			if (i % 100 == 0)
			{
				for (int d=0; d<dimen; d++)
				{
					r[d] = uniform_gen(low[d],up[d],ext[d]);
					rl[d] = r[d] - ext[d]/2;
					ru[d] = r[d] + ext[d]/2;
				}
			}
			else
			{
				for (int d=0; d<dimen; d++)
				{
					switch (type)
					{
					case 'u': case 'U':
						{
							r[d] = uniform_gen(low[d],up[d],ext[d]);
							break;
						}
					case 'g': case 'G':
						{
							r[d] = gaussian_gen(low[d],up[d],mean[d],var[d],ext[d]);
							break;
						}
					case 'c': case 'C':
						{
							int c = rand()%numcluster;
							//r[d] = gaussian_gen(clow[d][c],cup[d][c],mean[d],var[d],ext[d]);
							r[d] = gaussian_gen(low[d], up[d], (clow[d][c]+cup[d][c])/2,var[d],ext[d]);
							break;
						}
					default:
						fprintf(stderr,"invalid type of distribution %c.\n",type);
						exit(0);
					}
					rl[d] = r[d] - ext[d]/2;
					ru[d] = r[d] + ext[d]/2;
				}
			}
			for (int c=0; c<dimen && !valid; c++)
				if (ru[c] < exc[c] || exc[c] < rl[c])
					valid = true;
		}
		if (query)
		{
			for (int d=0; d<dimen; d++)
				fprintf(stdout,"%7.2f ",r[d]);
			fprintf(stdout,"\n");
		}
		else
		{
			if (point)
			{

				fprintf(stdout,"%d\t",i+1);
				for (int d=0; d<dimen-1; d++)
					fprintf(stdout,"%f\t",r[d]);
				fprintf(stdout,"%f\n",r[dimen-1]);
			}
			else
			{
				fprintf(stdout,"%7d ",i+1);
				char ls[256],us[256];


				sprintf(ls,"%7.2f",rl[0]);
				sprintf(us,"%7.2f",ru[0]);
				for (int d=1; d<dimen; d++)
				{
					char lts[32],uts[32];
					sprintf(lts," %7.2f",rl[d]);
					strcat(ls,lts);
					sprintf(uts," %7.2f",ru[d]);
					strcat(us,uts);
				}
				fprintf(stdout,"%s %s\n",ls,us);
			}
		}
	}
	return 0;
}


