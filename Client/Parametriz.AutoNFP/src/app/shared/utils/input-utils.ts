export class InputUtils {
  public static getInputValue(event: Event): string {
    return (event.target as HTMLInputElement).value;
  }
}