#include <vector>
#include <cmath>
#include "tgaimage.h"
#include "model.h"
#include "geometry.h"

const TGAColor white = TGAColor(255, 255, 255, 255);
const TGAColor red   = TGAColor(255, 0,   0,   255);
const TGAColor green = TGAColor(0,   255, 0,   255);

Model *model = NULL;
//const int width  = 2000;
//const int height = 2000;
const int width  = 200;
const int height = 200;

void line(int x0, int y0, int x1, int y1, TGAImage &image, TGAColor color) {
    bool steep = false;
    if (std::abs(x0-x1)<std::abs(y0-y1)) {
        std::swap(x0, y0);
        std::swap(x1, y1);
        steep = true;
    }
    if (x0>x1) {
        std::swap(x0, x1);
        std::swap(y0, y1);
    }

    for (int x=x0; x<=x1; x++) {
        float t = (x-x0)/(float)(x1-x0);
        int y = y0*(1.-t) + y1*t;
        if (steep) {
            image.set(y, x, color);
        } else {
            image.set(x, y, color);
        }
    }
}

void line(Vec2i p0, Vec2i p1, TGAImage &image, TGAColor color) {
    bool steep = false;
    if (std::abs(p0.x-p1.x)<std::abs(p0.y-p1.y)) {
        std::swap(p0.x, p0.y);
        std::swap(p1.x, p1.y);
        steep = true;
    }
    if (p0.x>p1.x) {
        std::swap(p0, p1);
    }
    
    for (int x=p0.x; x<=p1.x; x++) {
        float t = (x-p0.x)/(float)(p1.x-p0.x);
        int y = p0.y*(1.-t) + p1.y*t;
        if (steep) {
            image.set(y, x, color);
        } else {
            image.set(x, y, color);
        }
    }
}


void triangle(Vec2i t0, Vec2i t1, Vec2i t2, TGAImage &image, TGAColor color) {
    // sort the vertices, t0, t1, t2 lower−to−upper (bubblesort yay!)
    if (t0.y>t1.y) std::swap(t0, t1);
    if (t0.y>t2.y) std::swap(t0, t2);
    if (t1.y>t2.y) std::swap(t1, t2);
//    line(t0, t1, image, green);
//    line(t1, t2, image, green);
//    line(t2, t0, image, red);
    int total_height = t2.y-t0.y;
    for (int y=t0.y; y<=t1.y; y++) {
        int segment_height = t1.y-t0.y+1;
        float alpha = (float)(y-t0.y)/total_height;
        float beta  = (float)(y-t0.y)/segment_height; // be careful with divisions by zero
        Vec2i A = t0 + (t2-t0)*alpha;
        Vec2i B = t0 + (t1-t0)*beta;
//        image.set(A.x, y, red);
//        image.set(B.x, y, green);
        if (A.x>B.x) std::swap(A, B);
        for (int j=A.x; j<=B.x; j++) {
            image.set(j, y, color); // attention, due to int casts t0.y+i != A.y
        }
        
        for (int y=t1.y; y<=t2.y; y++) {
            int segment_height =  t2.y-t1.y+1;
            float alpha = (float)(y-t0.y)/total_height;
            float beta  = (float)(y-t1.y)/segment_height; // be careful with divisions by zero
            Vec2i A = t0 + (t2-t0)*alpha;
            Vec2i B = t1 + (t2-t1)*beta;
            if (A.x>B.x) std::swap(A, B);
            for (int j=A.x; j<=B.x; j++) {
                image.set(j, y, color); // attention, due to int casts t0.y+i != A.y
            }
        } 
    }
}

int main(int argc, char** argv) {
    if (2==argc) {
        model = new Model(argv[1]);
    } else {
        model = new Model("/Users/wei_li/Git/my-tinyrenderer/TinyRenderer/obj/african_head.obj");
    }

    TGAImage image(width, height, TGAImage::RGB);
    
//    for (int i=0; i<model->nfaces(); i++) {
//        std::vector<int> face = model->face(i);
//        for (int j=0; j<3; j++) {
//            Vec3f v0 = model->vert(face[j]);
//            Vec3f v1 = model->vert(face[(j+1)%3]);
//            int x0 = (v0.x+1.)*width/2.;
//            int y0 = (v0.y+1.)*height/2.;
//            int x1 = (v1.x+1.)*width/2.;
//            int y1 = (v1.y+1.)*height/2.;
//            line(x0, y0, x1, y1, image, white);
//        }
//    }
    
    Vec2i t0[3] = {Vec2i(10, 70),   Vec2i(50, 160),  Vec2i(70, 80)};
    Vec2i t1[3] = {Vec2i(180, 50),  Vec2i(150, 1),   Vec2i(70, 180)};
    Vec2i t2[3] = {Vec2i(180, 150), Vec2i(120, 160), Vec2i(130, 180)};
    triangle(t0[0], t0[1], t0[2], image, red);
    triangle(t1[0], t1[1], t1[2], image, white);
    triangle(t2[0], t2[1], t2[2], image, green);

    image.flip_vertically(); // i want to have the origin at the left bottom corner of the image
    image.write_tga_file("output.tga");
    
    delete model;
    return 0;
}
