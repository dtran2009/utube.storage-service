# Storage Service

[![Docker Image CI](https://github.com/letslearn373/utube.storage-service/actions/workflows/docker-image.yml/badge.svg)](https://github.com/letslearn373/utube.storage-service/actions/workflows/docker-image.yml)

This service is responsible for managing files in the storage. As of now, this uses [Minio](https://min.io/) for storing files.

## :hammer_and_wrench: Environment variables

These settings can be set in the `appsettings.json` file or can be overridden using environment variable.

| Varialble name | Usage | Example |
| -------------- | ----- | ------- |
| `MinioSetting.Endpoint` | Minio storage endpoint | `localhost:6000` |
| `MinioSetting.AccessKey` | Access key | `zAN*********FDPh` |
| `MinioSetting.Secretkey` | Secret key | `Fy2QKQ*******FYs1lMu` |
| `MinioSetting.UseSSL` | Should use SSL or not | `false` |
| `MinioSetting.BucketName` | Object bucket name | `my-bucket` |
| `RabbitMQSetting.Endpoint` | RabbitMQ Endpoint | `localhost:15674` |
| `RabbitMQSetting.Username` | Username | `guest` |
| `RabbitMQSetting.Password` | Passord | `pass***d` |
| `RabbitMQSetting.VirtualHost` | Virtual host name, default `'/'` | `my-host` |

## :speech_balloon: Rest API Endpoints
* [File](#file)
    * [Upload](#upload)

### File
#### Upload
This api upload files to the Minio storage.
```js
POST /file/upload
```
##### Arguments (Form Submission)
| Name | Required | Value |
| ---- | -------- | ----- |
| `inputFile` | true | This is the file that needs to be uploaded |


##### Response Sample
```json
{
    "videoId": "1c76e18a-919d-41e2-bcb5-d75ff963da24",
    "videoPath": "1c76e18a-919d-41e2-bcb5-d75ff963da24/1c76e18a-919d-41e2-bcb5-d75ff963da24.mp4"
}
```

## :dash: gRPC Endpoints
* `UploadFile` - This endpoint takes file in stream along with **videoId**, **mimeType**, **upload type** (Video, thumbnail, fhd, hd, sd). Uploaded file will be stored in the storage based on defined type.

## 	:loudspeaker: Events
| Event Name | Type | Purpose |
| ---------- | ----- | ------- |
| `VideoUploadedEvent` | Publish | This event is published when the file is successfully uploaded into the storage |
