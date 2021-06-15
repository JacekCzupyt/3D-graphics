# 3D-graphics

My soluton to a small 3d-graphics project from my computer graphics course.

## Task description

Create a program with a graphical user interface for displaying in the
program window a render of a 3D scnene.
The application should allow the user to control a virtual camera which
will determine the point of view the scene is observed from. The minimum
requirement is to have a camera pointed at a centre of the scene that allow
the user to change the distance from the centre and rotate it around X and
Y axes.

Your program should load the objects to display in
the scene from a file. The file format should be of your design. Within
it for each object you should store information about:

• object type, which should be either: cylinder, sphere, cuboid or
a cone;

• object size, e.g. base radius and height for cone and cylinder,
radius for a sphere, edge lengths for a cuboid;

• desired mesh density, i.e. a number of subdivisions when approximating the shape with a triangular mesh (not necessary for a
cuboid);

• position and orientation of the object in the scene expressed as a
single affine transformation matrix

Program should generate meshes for the objects and place them in the
scene according to the loaded information. When displaying the scene
you can draw object wireframes, i.e. it is enough to draw edges of
triangles of each mesh projected onto the screen.

## How to use

Move around using wasdrf keys. Look around by holding the right mouse button.

You can create a new scene as a json file. Look to test6.json for an example. Then load it using the load menu button.
