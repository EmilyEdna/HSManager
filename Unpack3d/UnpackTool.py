import io
import string
import UnityPy
import base64
from zipfile import ZipFile

class UnpackTool(object):

    def __init__(self) -> None:
        pass

    def UnpackUnityAssest(unityFile:str)->list:
        bytesArr =[]

        if not unityFile.endswith(".unity3d"):
         return bytesArr

        env = UnityPy.load(unityFile)

        for index,obj in enumerate(env.objects):

            if(obj.type.name in  ["Texture2D", "Sprite"]):
                data = obj.read()
                imgByteArr = io.BytesIO()
                data.image.save(imgByteArr,format="png")
                b64=base64.b64encode(imgByteArr.getvalue())
                bytesArr.append(b64)

        return bytesArr


    def UnpackUnityAssestZipmod(self,unityFile:str)->str:
        bytesArr =[]
        if not unityFile.endswith(".zipmod"):
           return bytesArr

        with open(unityFile,"rb") as buffer:
            z = ZipFile(buffer)
            env = UnityPy.Environment()
            env.load_assets(z.namelist(),lambda x: z.open(x, "r"))

            for index,obj in enumerate(env.objects):

              if(obj.type.name in  ["Texture2D", "Sprite"]):
                data = obj.read()
                imgByteArr = io.BytesIO()
                data.image.save(imgByteArr,format="png")
                b64=base64.b64encode(imgByteArr.getvalue())
                bytesArr.append(b64)

            z.close()
        return ",".join(bytesArr)