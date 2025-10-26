import moment from "moment";

export class DataUtils {

  public static formatarParaParametro(data: any):string {
    if(data) {
      const dataMoment = moment(data);
      return dataMoment.local().format('MM-DD-YYYY');
    } return '';
  }
}