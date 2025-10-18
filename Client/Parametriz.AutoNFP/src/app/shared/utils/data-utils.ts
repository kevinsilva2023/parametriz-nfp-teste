import moment from "moment";

export class DataUtils {

  public static formatarParaParametro(data: any):string {
    const dataMoment = moment(data);
    return dataMoment.local().format('MM-DD-YYYY');
  }
}