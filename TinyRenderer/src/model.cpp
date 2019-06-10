#include <iostream>
#include <string>
#include <fstream>
#include <sstream>
#include <vector>

#include "model.h"//including "geometry.h"

Model::Model(const char *filename) : verts_(), faces_() {
    std::ifstream in; // input file stream class
    in.open (filename, std::ifstream::in); // open file, File open for reading
    if (in.fail()) {
        std::cerr << "File open fail!" << std::endl;
        return;
    }
    std::string line;
    while (!in.eof()) { // while not End-of-File...
        std::getline(in, line);
        std::istringstream iss(line.c_str()); // Input string stream
        char trash;
        if (!line.compare(0, 2, "v ")) { // (pos,len,str); vertex line
            iss >> trash;
            Vec3f v;
            for (int i=0;i<3;i++) iss >> v.raw[i];
            verts_.push_back(v);
        } else if (!line.compare(0, 2, "f ")) { // faces
            std::vector<int> f;
            int itrash, idx;
            iss >> trash;
            while (iss >> idx >> trash >> itrash >> trash >> itrash) {
                idx--; // in wavefront obj all indices start at 1, not zero
                f.push_back(idx);
            }
            faces_.push_back(f);
        }
    }
    std::cerr << "# v# " << verts_.size() << " f# "  << faces_.size() << std::endl;
}

Model::~Model() {
}


int Model::nverts() {
    return (int)verts_.size();
}
int Model::nfaces() {
    return (int)faces_.size();
}


Vec3f Model::vert(int i) {
    return verts_[i];
}
std::vector<int> Model::face(int idx) {
    return faces_[idx];
}
