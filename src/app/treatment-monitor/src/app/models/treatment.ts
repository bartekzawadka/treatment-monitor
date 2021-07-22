import Medicine from "./medicine";

export default class Treatment {
  public id = '';
  public name = '';
  public startDate = new Date();
  public terminated = false;
  public medicines: Medicine[] = [];
}
