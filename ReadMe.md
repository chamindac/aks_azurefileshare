We can mount Azure file share to containers in AKS as explained in dcumentation here, and we can use static vloume  mounting to use existing Azure file share. The documentation only explains how to setup static volume mount with Azure CLI. This is example code for using terraform provisioned Azure file share strage as a static volume mount in AKS, using kuberntes yaml.

[Refer blog post here for more details.](https://chamindac.blogspot.com/2024/05/mount-azure-storage-fileshare-created.html)
