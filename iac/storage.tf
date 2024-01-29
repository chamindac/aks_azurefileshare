resource "azurerm_storage_account" "fs" {
  name                             = "${var.PREFIX}${var.PROJECT}${replace(var.ENVNAME, "-", "")}fsst"
  resource_group_name              = azurerm_resource_group.instancerg.name
  location                         = azurerm_resource_group.instancerg.location
  account_tier                     = "Premium"
  account_replication_type         = "LRS"
  account_kind                     = "FileStorage" # Azure file shares on SSD based hardware https://learn.microsoft.com/en-us/azure/storage/files/storage-how-to-create-file-share?tabs=azure-portal#create-a-storage-account
  access_tier                      = "Hot"
  allow_nested_items_to_be_public  = false
  min_tls_version                  = "TLS1_2"
  cross_tenant_replication_enabled = false

  network_rules {
    default_action             = "Deny"
    bypass                     = ["Metrics", "AzureServices", "Logging"]
    ip_rules                   = ["${chomp(data.http.mytfip.response_body)}"]
    virtual_network_subnet_ids = [azurerm_subnet.aks.id]
  }
}

resource "azurerm_storage_share" "aks" {
  name                 = "aksfileshare"
  storage_account_name = azurerm_storage_account.fs.name
  access_tier          = "Premium"
  quota                = 200 # Size in GB
}