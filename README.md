# 1bitToPng
 Converting 1 bit sprites zx spectrum to PNG image.

Build the project or use the releases .exe file to convert part of the Zx Spectrum memory dump into a PNG image.

Command line arguments:
"w" is the width of each sprite in the source.
"h" is the height of each sprite in the source.
"atlas" - pack everything into a single atlas, otherwise each sprite will be saved as a separate image.
"inv" - inversion of paper and ink.
"trans" - background transparency of each output sprite.
"attr" - Use the color of each sprite. For this option, the attributes of each sprite must follow it.

If you are using a single dump file, you can specify command line arguments for that file. If you don't want to use command line arguments - you can specify the command line arguments in the file name. In this case, the last argument should be the name of your file: name.bin.

Example using the command line: "w16 name.bin h16 trans" - arguments can be entered in any order.
An example of using a file name: "w16 h16 atlas trans name.bin" - the arguments can also be placed in any order except the name with extension. The name with the file extension must always be the last name in the argument list.


.bin files must be located in the folder with the .exe file.
After running the .exe file with command line arguments or using arguments in the name of each file to be converted, you will receive command line images in the local Png folder.
