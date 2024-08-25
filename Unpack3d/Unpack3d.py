import sys
from UnpackTool import UnpackTool
     
if __name__=="__main__":
   file_path= sys.argv[1]
   print(file_path)
   if file_path.endswith(".zipmod"):
     imgbytes = UnpackTool.UnpackUnityAssestZipmod(file_path)
     print(imgbytes)
   else:
     imgbytes = UnpackTool.UnpackUnityAssest(file_path)
     print(imgbytes)


   