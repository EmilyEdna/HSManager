import sys
import UnpackTool
     
if __name__=="__main__":
   file_path=sys.argv[1]
   if file_path.endswith(".zipmod"):
     imgbytes = UnpackTool.UnpackTool().UnpackUnityAssestZipmod(file_path)
     print(imgbytes)
   else:
     imgbytes = UnpackTool.UnpackTool().UnpackUnityAssest(file_path)
     print(imgbytes)


   