package application.Controlador;

public class Cprincipal {
	
	private CVmenuprincipal vista ;
	
	public Cprincipal(CVmenuprincipal vista2) {
		this.vista = vista2;
		
		this.vista.getMIsalida().setOnAction( e -> salir() );
		
	}
	private void salir() {
		System.exit(0);
	}

}
