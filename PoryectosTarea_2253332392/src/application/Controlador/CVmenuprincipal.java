package application.Controlador;

import javafx.fxml.FXML;
import javafx.scene.control.MenuBar;
import javafx.scene.control.MenuItem;

public class CVmenuprincipal {
	@FXML
	private MenuBar Mprincipal ;
	@FXML
	private MenuItem MIsalida, MIStopWatch , MIRelojDual , MILoanAssitant , MIInventory , MIMonitorPeso , MIMultpleChoiceExam;
	
	
	public MenuItem getMIsalida() {
		return this.MIsalida;
	}

	public MenuBar getMprincipal() {
		return Mprincipal;
	}

	public MenuItem getMIStopWatch() {
		return MIStopWatch;
	}

	public MenuItem getMIRelojDual() {
		return MIRelojDual;
	}

	public MenuItem getMILoanAssitant() {
		return MILoanAssitant;
	}

	public MenuItem getMIInventory() {
		return MIInventory;
	}

	public MenuItem getMIMonitorPeso() {
		return MIMonitorPeso;
	}

	public MenuItem getMIMultpleChoiceExam() {
		return MIMultpleChoiceExam;
	}


}
