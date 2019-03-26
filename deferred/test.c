GLuint depthBuf, posTex, normTex, colorTex;
// Create and bind the FBO, frame buffer object
glGenFramebuffers(1, &deferredFBO);
glBindFramebuffer(GL_FRAMEBUFFER, deferredFBO);
// Create and bind The depth buffer
glGenRenderbuffers(1, &depthBuf);
glBindRenderbuffer(GL_RENDERBUFFER, depthBuf);

glRenderbufferStorage(
    GL_RENDERBUFFER,
	GL_DEPTH_COMPONENT,
	width, height);

// Create and bind The position buffer
glActiveTexture(GL_TEXTURE0); // Use texture unit 0 for position buffer
glGenTextures(1, &posTex);
glBindTexture(GL_TEXTURE_2D, posTex);
glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB32F, width,
	height, 0,GL_RGB, GL_UNSIGNED_BYTE, NULL);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
	GL_NEAREST);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
	GL_NEAREST);

// Create and bind The normal buffer
glActiveTexture(GL_TEXTURE1); // Use texture unit 1 for normal buffer
glGenTextures(1, &normTex);
glBindTexture(GL_TEXTURE_2D, normTex);
glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB32F, width,
	height, 0,GL_RGB, GL_UNSIGNED_BYTE, NULL);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
	GL_NEAREST);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
	GL_NEAREST);

// The diffuse color (reflectivity) buffer
glActiveTexture(GL_TEXTURE2); // Texture unit 2 for diffuse color buffer
glGenTextures(1, &colorTex);
glBindTexture(GL_TEXTURE_2D, colorTex);
glTexImage2D(GL_TEXTURE_2D, 0, GL_RGB, width, height,
	0,GL_RGB, GL_UNSIGNED_BYTE, NULL);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER,
	GL_NEAREST);
glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER,
	GL_NEAREST);

// Attach the images to the framebuffer
glFramebufferRenderbuffer(GL_FRAMEBUFFER,
	GL_DEPTH_ATTACHMENT,GL_RENDERBUFFER,
	depthBuf);
glFramebufferTexture2D(GL_FRAMEBUFFER,
	GL_COLOR_ATTACHMENT0,GL_TEXTURE_2D,
	posTex, 0);
glFramebufferTexture2D(GL_FRAMEBUFFER,
	GL_COLOR_ATTACHMENT1,GL_TEXTURE_2D,
	normTex, 0);
glFramebufferTexture2D(GL_FRAMEBUFFER,
	GL_COLOR_ATTACHMENT2,GL_TEXTURE_2D,
	colorTex, 0);
GLenumdrawBuffers[] = {GL_NONE, GL_COLOR_ATTACHMENT0,
	GL_COLOR_ATTACHMENT1,GL_COLOR_ATTACHMENT2};

glDrawBuffers(4, drawBuffers);
