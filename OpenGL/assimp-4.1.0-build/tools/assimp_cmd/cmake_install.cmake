# Install script for directory: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0/tools/assimp_cmd

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "/usr/local")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "Release")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xassimp-binx" OR NOT CMAKE_INSTALL_COMPONENT)
  if("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Dd][Ee][Bb][Uu][Gg])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE EXECUTABLE FILES "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Debug/assimp")
    if(EXISTS "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp" AND
       NOT IS_SYMLINK "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND "/usr/bin/install_name_tool"
        -change "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.4.dylib" "/usr/local/lib/libassimp.4.dylib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/lib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      if(CMAKE_INSTALL_DO_STRIP)
        execute_process(COMMAND "/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/strip" -u -r "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      endif()
    endif()
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ee][Aa][Ss][Ee])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE EXECUTABLE FILES "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Release/assimp")
    if(EXISTS "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp" AND
       NOT IS_SYMLINK "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND "/usr/bin/install_name_tool"
        -change "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.4.dylib" "/usr/local/lib/libassimp.4.dylib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/lib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      if(CMAKE_INSTALL_DO_STRIP)
        execute_process(COMMAND "/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/strip" -u -r "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      endif()
    endif()
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Mm][Ii][Nn][Ss][Ii][Zz][Ee][Rr][Ee][Ll])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE EXECUTABLE FILES "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/MinSizeRel/assimp")
    if(EXISTS "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp" AND
       NOT IS_SYMLINK "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND "/usr/bin/install_name_tool"
        -change "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.4.dylib" "/usr/local/lib/libassimp.4.dylib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/lib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      if(CMAKE_INSTALL_DO_STRIP)
        execute_process(COMMAND "/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/strip" -u -r "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      endif()
    endif()
  elseif("${CMAKE_INSTALL_CONFIG_NAME}" MATCHES "^([Rr][Ee][Ll][Ww][Ii][Tt][Hh][Dd][Ee][Bb][Ii][Nn][Ff][Oo])$")
    file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/bin" TYPE EXECUTABLE FILES "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/RelWithDebInfo/assimp")
    if(EXISTS "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp" AND
       NOT IS_SYMLINK "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND "/usr/bin/install_name_tool"
        -change "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.4.dylib" "/usr/local/lib/libassimp.4.dylib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      execute_process(COMMAND /usr/bin/install_name_tool
        -delete_rpath "/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/lib"
        "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      if(CMAKE_INSTALL_DO_STRIP)
        execute_process(COMMAND "/Applications/Xcode.app/Contents/Developer/Toolchains/XcodeDefault.xctoolchain/usr/bin/strip" -u -r "$ENV{DESTDIR}${CMAKE_INSTALL_PREFIX}/bin/assimp")
      endif()
    endif()
  endif()
endif()

