# Shopaholic Project
購物網站Sample，包含以下3個Web app，使用ASP .NET Core(.NET 6) + Vue.js，並在AWS EC2 Ubuntu架站

## Shopaholic.CMS (後台管理介面)
https://ec2-3-114-141-160.ap-northeast-1.compute.amazonaws.com:13771  
* 管理商品類別
* 商品上架，藉由summernote富文字編輯器來編輯商品文案，並串接imgur api來上傳圖片
* 訂單管理、搜索
* 計算前台購物網站瀏覽流量

## Shopaholic.Web (前台購物網站展示)
https://ec2-3-114-141-160.ap-northeast-1.compute.amazonaws.com:13971  
* 用nginx作load balancer，將程式流量分配至2個Server
* 串接Firebase Auth 來達成第三方登入(Google)，並整合Cookie-based Authentication
* 串接Line pay支付
* 商品搜索及後端分頁
* 計算最多人購買及瀏覽商品

## Shopaholic.BackgroundWorker (Background Service)
* 背景服務 .Net BackgroundService
* 定時將未付款訂單改為逾期
* 定時模擬訂單出貨
