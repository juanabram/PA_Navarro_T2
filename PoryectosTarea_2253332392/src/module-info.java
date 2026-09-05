module PoryectosTarea_2253332392 {
	requires javafx.controls;
	requires javafx.fxml;
	
	exports application;
	opens application to javafx.graphics, javafx.fxml;
	opens application.Controlador to javafx.fxml;
}
