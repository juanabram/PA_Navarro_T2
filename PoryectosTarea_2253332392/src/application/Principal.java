package application;

import javafx.stage.Stage;
import javafx.scene.Scene;
import javafx.scene.Parent;
import application.Controlador.CVmenuprincipal;
import application.Controlador.Cprincipal;
import javafx.fxml.FXMLLoader;


public class Principal extends javafx.application.Application {
	
	@Override
	public void start(Stage stage) throws Exception {
		
		FXMLLoader loader = new FXMLLoader (
				getClass().getResource("Vista/Vmenuprincipal.fxml")
				);
		
		Parent root = loader.load();
		
		CVmenuprincipal vista = loader.getController();
		Cprincipal controlador = new Cprincipal(vista);
		stage.setScene(new Scene(root));
		stage.show();
		
	}
	
	public static void main(String[] args) {
		launch(args);
	}
}
