class FormEvent:
    def __init__(self, source, name, occupation, ageCat, empCat, taxId, usCitizen):
        self.source = source
        self.name = name
        self.occupation = occupation
        self.ageCategory = ageCat
        self.empCat = empCat
        self.taxId = taxId
        self.usCitizen = usCitizen

    def getName(self): return self.name
    def getOccupation(self): return self.occupation
    def getAgeCategory(self): return self.ageCategory
    def getEmploymentCategory(self): return self.empCat
    def getTaxId(self): return self.taxId
    def isUsCitizen(self): return self.usCitizen