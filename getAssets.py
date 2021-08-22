import boto3
import os


class AssetHandler:
    def __init__(self, bucket_name, profile_name):
        self._bucket_name = bucket_name
        self.session = boto3.Session(profile_name=profile_name)
        self.client = self.session.client('s3')
        self.download_folder = "DownloadedAssets"
        self.upload_folder = "UploadAssets"

    def download_contents(self):
        objects = self.client.list_objects(
            Bucket=self._bucket_name, Prefix="Assets/")
        print(objects['Contents'])

        for object in objects['Contents']:
            if object['Key'] == 'Assets/':
                continue
            else:
                filename = object['Key'].split('/')[-1]
                self.client.download_file(
                    self._bucket_name, object['Key'], self.download_folder + "/" + filename)

    def upload_contents(self):
        arr = os.listdir("./UploadAssets")
        cwd = os.getcwd()

        for file in arr:
            if (file != ".gitkeep"):
                response = self.client.upload_file(
                    cwd + "/UploadAssets/" + file, self._bucket_name, "Assets/" + file)


def main():
    handler = AssetHandler('brackeysgamejam2021', 'default')
    choice = int(input("Press 1 to upload, 2 to download\n"))

    if choice == 1:
        handler.upload_contents()
    elif choice == 2:
        handler.download_contents()
    else:
        print("are you stupid?")


main()
