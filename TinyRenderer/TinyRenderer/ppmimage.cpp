#include <iostream>
#include <fstream>
#include <string.h>
#include <time.h>
#include <math.h>
#include "tgaimage.h"

void 
int nx = 200;
int ny = 100;
//nx = 800;
//ny = 400;
int ns = 100;

ofstream outfile("test.ppm", ios_base::out);
outfile << "P3\n" << nx << " " << ny << "\n255\n";
//std::cout << "P3\n" << nx << " " << ny << "\n255\n";

for (int j = ny - 1; j >= 0; j--) {
	for (int i = 0; i<nx; i++) {
		counter++;
		vec3 col(0, 0, 0);
		for (int s = 0; s < ns; s++) {
			float u = float(i + rand() % (100) / (float)(100)) / float(nx); // 0~1
			float v = float(j + rand() % (100) / (float)(100)) / float(ny);
			// float u = float(i) / float(nx);
			// float v = float(j) / float(ny);
			ray r = cam.get_ray(u, v); // generate ray per sample

									   // old test
									   // vec3 p = r.point_at_parameter(1.0);
									   // p = unit_vector(p);
									   // col = vec3(p.z(),p.z(),p.z());
									   // col += color(r);

			col += color(r, world, 0);
		}

		int mod = counter / (total / 100);

		if (current != mod) {
			current = mod;
			//cout << "*";
			cout << '\b' << '\b' << '\b' << current << "%";
			//cout << counter << endl;
		}

		col /= float(ns);

		// gamma correction
		col = vec3(sqrt(col[0]), sqrt(col[1]), sqrt(col[2]));

		int ir = int(255.99*col[0]);
		int ig = int(255.99*col[1]);
		int ib = int(255.99*col[2]);

		outfile << ir << " " << ig << " " << ib << "\n";
		//std::cout << ir << " " << ig << " " << ib << "\n";
	}
}
cout << endl;
cout << "Image output succeeded! :)" << "\n";
//system("pause");