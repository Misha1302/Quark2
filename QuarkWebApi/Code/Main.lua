import "../Libraries"

// // // Enter point \\ \\ \\
def Main() {
    SetStatic("GlobalData", CreateMap())
    _ = LoadNotesFromDrive()

    _ = AddPostEndpoint("AddNote")
    _ = AddGetEndpoint("PrintNotes")
    _ = AddPutEndpoint("UpdateTextInNote")
    _ = AddDeleteEndpoint("DeleteNote", 1)
    _ = AddDeleteEndpoint("DeleteAll", 0)

    return 0
}





// requests examples:
// add note: { "name":"1", "text":"111" }
// print nodes
// update text in note: { "name":"1", "text":"new text" }
// delete note: 1





// // // Application request handlers \\ \\ \\

// Create
def AddNote(text) {
    map = DeserializeIntoMap(text)
    if not IsValidNodeMap(map) { return "Invalid enter data" }    
    SetMapValue(Get("Notes"), GetMapValue(map, "name"), map)
    _ = Save()
    return "Added"
}

// Read
def PrintNotes() {
    return SerializeMap(Get("Notes"))
}

// Update
def UpdateTextInNote(text) {
    // do the same as add note function
    result = AddNote(text)
    if result == "Added" { return "Updated" }
    return result
}

// Delete
def DeleteNote(text) {
    RemoveMapValue(Get("Notes"), text)
    _ = Save()
    return "Removed"
}

// Delete
def DeleteAll(text) {
    _ = Set("Notes", CreateMap())
    _ = Save()
    return "Removed all notes"
}





// // // Enter data validators \\ \\ \\
def IsValidNodeMap(map) {
    if IsNil(map) { return 0 }
    if not MapContains(map, "name") { return 0 }
    if not MapContains(map, "text") { return 0 }

    return 1
}





// // // Save changes to drive \\ \\ \\
def Save() {
    map = Get("Notes")
    str = SerializeMap(map)
    WriteText("Code/NotesJsonFile", str)
    return 0
}

def LoadNotesFromDrive() {
    SetMapValue(GetStatic("GlobalData"), "Notes", DeserializeIntoMapOfMaps(ReadText("Code/NotesJsonFile")))
    return 0
}





// // // Application global data getter/setter \\ \\ \\
def Set(name, value) {
    SetMapValue(GetStatic("GlobalData"), name, value)
    return 0
}

def Get(name) {
    return GetMapValue(GetStatic("GlobalData"), name)
}





// // // Platform Calls \\ \\ \\
def AddGetEndpoint(name) {
    __platform_call("AddGetEndpoint", name, 1)
    return 0
}

def AddPostEndpoint(name) {
    __platform_call("AddPostEndpoint", name, 1)
    return 0
}

def AddDeleteEndpoint(name, needArgument) {
    __platform_call("AddDeleteEndpoint", name, needArgument, 2)
    return 0
}

def AddPutEndpoint(name) {
    __platform_call("AddPutEndpoint", name, 1)
    return 0
}