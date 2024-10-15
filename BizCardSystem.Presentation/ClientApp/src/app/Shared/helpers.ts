export function getTimeStamp() {
    let currentDate = new Date();
    let year = currentDate.getFullYear();
    let month = ("0" + (currentDate.getMonth() + 1)).slice(-2);
    let day = ("0" + currentDate.getDate()).slice(-2);
    let hours = ("0" + currentDate.getHours()).slice(-2);
    let minutes = ("0" + currentDate.getMinutes()).slice(-2);
    let seconds = ("0" + currentDate.getSeconds()).slice(-2);
  
    return year + "_" + month + "_" + day + "_" + hours + "_" + minutes + "_" + seconds;
  }  
  export class FileType {
    Extension?:string;
    Type?:string;
   constructor(extension?:string,type?:string) {
      this.Type=type;
      this.Extension=extension; 
   }
  }

  export  class FileTypes {
    public static csv:FileType=new FileType(".csv","text/csv");
    public static xml:FileType=new FileType(".xml","application/xml");
   }

export function DownloadFile(data:Blob,type:string,extension:string) 
{
    const blob: Blob = new Blob([data], { type: type });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = 'business_card' + getTimeStamp() + extension;
    link.click();
    window.URL.revokeObjectURL(url);
}

 
