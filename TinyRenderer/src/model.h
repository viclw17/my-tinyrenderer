#ifndef __MODEL_H__
#define __MODEL_H__

#include <vector>
#include "geometry.h"

class Model {
public:
	Model(const char *filename);
	~Model();
    
    // get vert/face total amount
	int nverts();
	int nfaces();
    
    // get vertex/face by index
	Vec3f vert(int i);
	std::vector<int> face(int idx);
    
private:
    std::vector<Vec3f> verts_;
    std::vector<std::vector<int> > faces_;
};

#endif //__MODEL_H__
