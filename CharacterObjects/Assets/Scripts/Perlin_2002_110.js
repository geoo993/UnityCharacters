//Improving Noise - Orginal by Ken Perlin (SIGGRAPH 2002)

///*
//var noise: Perlin_2002;	
//...	
//if (!noise) 
//{ 
//	noise = GetComponent(Perlin_2002);
//	noise.setup();			// use lookup table
//	//noise.setup2(seed);	// use system radom
//}
//...
//var n = noise.Noise(x, y, z);
//*/
//
//private var p: int[] = new int[512]; 
//private var permutation: int[] = [ 151,160,137,91,90,15,
//   131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
//   190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
//   88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
//   77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
//   102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
//   135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
//   5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
//   223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
//   129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
//   251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
//   49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
//   138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
//   ];
//
//function Noise(x: float, y: float, z: float): float
//{	
//	var X: int = parseInt(Mathf.Floor(x)) & 255;
//	var Y: int = parseInt(Mathf.Floor(y)) & 255;
//	var Z: int = parseInt(Mathf.Floor(z)) & 255;
//	
//	x -= Mathf.Floor(x);
//	y -= Mathf.Floor(y);
//	z -= Mathf.Floor(z);
//      
//	var u: float = fade(x);
//	var	v: float = fade(y);
//	var	w: float = fade(z);
//	
//	var A:  int	= p[X  	]+Y;
//	var AA: int = p[A	]+Z;
//	var AB: int = p[A+1	]+Z; 
//	var B:  int = p[X+1	]+Y;
//	var BA: int = p[B	]+Z;
//	var BB: int = p[B+1	]+Z;
//
//	return lerp(w, lerp(v, lerp(u, grad(p[AA  ], x  , y  , z   ),
//                                   grad(p[BA  ], x-1, y  , z   )),
//                           lerp(u, grad(p[AB  ], x  , y-1, z   ),
//                                   grad(p[BB  ], x-1, y-1, z   ))),
//                   lerp(v, lerp(u, grad(p[AA+1], x  , y  , z-1 ),
//                                   grad(p[BA+1], x-1, y  , z-1 )),
//                           lerp(u, grad(p[AB+1], x  , y-1, z-1 ),
//                                   grad(p[BB+1], x-1, y-1, z-1 ))));
//}
//
//function fade(t: float): float { return t * t * t * (t * (t * 6.0 - 15.0) + 10.0); }
//function lerp(t: float,  a: float, b: float): float { return a + t * (b - a); }
//
//function grad(hash: int, x: float, y: float, z: float): float
//{
//	var h: int = hash & 15;
//	var u: float = h<8 ? x : y;
//	var v: float = h<4 ? y : (h==12||h==14 ? x : z);
//	return ((h&1) == 0 ? u : -u) + ((h&2) == 0 ? v : -v);
//}
//	
//function setup() 
//{
//	print("-> init");	
//	for (var i: int=0; i < 256 ; i++) { p[256+i] = p[i] = permutation[i]; }
//}
//
//function setup2(s) 
//{
//	print("-> init2");
//	var rnd = new Random();
//	rnd.seed = s;
//	for (var i: int=0; i < 256 ; i++) { p[256+i] = p[i] = parseInt(rnd.value * 255); }
//}
