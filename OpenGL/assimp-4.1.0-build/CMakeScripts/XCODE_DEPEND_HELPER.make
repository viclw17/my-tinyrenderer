# DO NOT EDIT
# This makefile makes sure all linkable targets are
# up-to-date with anything they link to
default:
	echo "Do not invoke directly"

# Rules to remove targets that are older than anything to which they
# link.  This forces Xcode to relink the targets from scratch.  It
# does not seem to check these dependencies itself.
PostBuild.IrrXML.Debug:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Debug/libIrrXML.a:
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Debug/libIrrXML.a


PostBuild.assimp.Debug:
PostBuild.IrrXML.Debug: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.dylib
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.dylib:\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Debug/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.dylib


PostBuild.assimp_cmd.Debug:
PostBuild.assimp.Debug: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Debug/assimp
PostBuild.IrrXML.Debug: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Debug/assimp
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Debug/assimp:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Debug/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Debug/assimp


PostBuild.unit.Debug:
PostBuild.assimp.Debug: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Debug/unit
PostBuild.IrrXML.Debug: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Debug/unit
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Debug/unit:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Debug/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Debug/unit


PostBuild.IrrXML.Release:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Release/libIrrXML.a:
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Release/libIrrXML.a


PostBuild.assimp.Release:
PostBuild.IrrXML.Release: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.dylib
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.dylib:\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Release/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.dylib


PostBuild.assimp_cmd.Release:
PostBuild.assimp.Release: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Release/assimp
PostBuild.IrrXML.Release: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Release/assimp
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Release/assimp:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Release/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/Release/assimp


PostBuild.unit.Release:
PostBuild.assimp.Release: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Release/unit
PostBuild.IrrXML.Release: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Release/unit
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Release/unit:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Release/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/Release/unit


PostBuild.IrrXML.MinSizeRel:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/MinSizeRel/libIrrXML.a:
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/MinSizeRel/libIrrXML.a


PostBuild.assimp.MinSizeRel:
PostBuild.IrrXML.MinSizeRel: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.dylib
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.dylib:\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/MinSizeRel/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.dylib


PostBuild.assimp_cmd.MinSizeRel:
PostBuild.assimp.MinSizeRel: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/MinSizeRel/assimp
PostBuild.IrrXML.MinSizeRel: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/MinSizeRel/assimp
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/MinSizeRel/assimp:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/MinSizeRel/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/MinSizeRel/assimp


PostBuild.unit.MinSizeRel:
PostBuild.assimp.MinSizeRel: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/MinSizeRel/unit
PostBuild.IrrXML.MinSizeRel: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/MinSizeRel/unit
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/MinSizeRel/unit:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/MinSizeRel/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/MinSizeRel/unit


PostBuild.IrrXML.RelWithDebInfo:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/RelWithDebInfo/libIrrXML.a:
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/RelWithDebInfo/libIrrXML.a


PostBuild.assimp.RelWithDebInfo:
PostBuild.IrrXML.RelWithDebInfo: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.dylib
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.dylib:\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/RelWithDebInfo/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.dylib


PostBuild.assimp_cmd.RelWithDebInfo:
PostBuild.assimp.RelWithDebInfo: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/RelWithDebInfo/assimp
PostBuild.IrrXML.RelWithDebInfo: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/RelWithDebInfo/assimp
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/RelWithDebInfo/assimp:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/RelWithDebInfo/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/tools/assimp_cmd/RelWithDebInfo/assimp


PostBuild.unit.RelWithDebInfo:
PostBuild.assimp.RelWithDebInfo: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/RelWithDebInfo/unit
PostBuild.IrrXML.RelWithDebInfo: /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/RelWithDebInfo/unit
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/RelWithDebInfo/unit:\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.4.1.0.dylib\
	/usr/lib/libz.dylib\
	/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/RelWithDebInfo/libIrrXML.a
	/bin/rm -f /Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/test/RelWithDebInfo/unit




# For each target create a dummy ruleso the target does not have to exist
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Debug/libassimp.4.1.0.dylib:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/MinSizeRel/libassimp.4.1.0.dylib:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/RelWithDebInfo/libassimp.4.1.0.dylib:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/code/Release/libassimp.4.1.0.dylib:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Debug/libIrrXML.a:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/MinSizeRel/libIrrXML.a:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/RelWithDebInfo/libIrrXML.a:
/Users/wei_li/Git/my-tinyrenderer/OpenGL/assimp-4.1.0-build/contrib/irrXML/Release/libIrrXML.a:
/usr/lib/libz.dylib:
