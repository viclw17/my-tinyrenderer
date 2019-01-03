#include <iostream>
#include <vector>
#include <cmath>

#include "tgaimage.h"
#include "model.h"
#include "geometry.h"
using namespace std;

const TGAColor white = TGAColor(255, 255, 255, 255);
const TGAColor red   = TGAColor(255, 0,   0,   255);
Model *model = NULL;
const int width = 800;
const int height = 800;

void line(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color);
void line2(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color);
void line3(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color);

int main(int argc, char** argv) {
	//TGAImage image(100, 100, TGAImage::RGB);
	// draw lines
    //line(13, 20, 80, 40, image, white); 
    //line2(13, 20, 80, 40, image, white);
    //line3(20, 13, 40, 80, image, red);
    //line3(80, 40, 13, 20, image, red);


	if (2 == argc) {
		model = new Model(argv[1]);
	}
	else {
		model = new Model("obj/african_head.obj");
	}

	TGAImage image(width, height, TGAImage::RGB);
	for (int i = 0; i < model->nfaces(); i++) {
		std::vector<int> face = model->face(i);
		for (int j = 0; j < 3; j++) {
			Vec3f v0 = model->vert(face[j]);
			Vec3f v1 = model->vert(face[(j + 1) % 3]);
			int x0 = (v0.x + 1.)*width / 2.;
			int y0 = (v0.y + 1.)*height / 2.;
			int x1 = (v1.x + 1.)*width / 2.;
			int y1 = (v1.y + 1.)*height / 2.;
			line(x0, y0, x1, y1, image, white);
		}
	}

	image.flip_vertically(); // i want to have the origin at the left bottom corner of the image
	image.write_tga_file("output.tga");
	delete model;

	//int x;
	//float y;
	//cout << sizeof(x) << " " << sizeof(y); // 4 4
	return 0;
}

void line(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color) {
	for (float t = 0; t < 1; t += 0.1) {
		int x = x0 * (1. - t) + x1 * t;
		int y = y0 * (1. - t) + y1 * t;
		image.set(x, y, color);
	}
}

void line2(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color) {
    for (int x = x0; x <= x1; x++) {
        float t = (x-x0) / (float)(x1-x0); // proportion
        int y = y0 * (1. - t) + y1 * t;
        image.set(x, y, color);
    }
}

void line3(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color) {
    bool steep = false;
    if (abs(x0-x1)<abs(y0-y1)) { // if the line is steep, we transpose the image
        swap(x0, y0);
        swap(x1, y1);
        steep = true;
    }
    if (x0>x1) { // make it left−to−right
        swap(x0, x1);
        swap(y0, y1);
    }
    for (int x=x0; x<=x1; x++) {
        float t = (x-x0)/(float)(x1-x0);
        int y = y0*(1.-t) + y1*t;
        if (steep) {
            image.set(y, x, color); // if transposed, de−transpose
        } else {
            image.set(x, y, color);
        }
    }
}
