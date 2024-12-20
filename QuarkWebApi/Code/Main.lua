import "../Libraries"

// // // Enter point \\ \\ \\
Number Main() {
    SetStatic("GlobalData", CreateMap())
    SetMapValue(GetStatic("GlobalData"), "Notes", CreateMap())

    AddPostEndpoint("AddNote")
    AddGetEndpoint("PrintNotes")
    AddPostEndpoint("UpdateTextInNote")
    AddPostEndpoint("DeleteNote")

    return 0
}




// // // Application request handlers \\ \\ \\

// Create
Any AddNote(text) {
    map = DeserializeIntoMap(text)
    SetMapValue(Get("Notes"), GetMapValue(map, "name"), map)
    return "Added"
}

// Read
Any PrintNotes() {
    return SerializeMap(Get("Notes"))
}

// Update
Any UpdateTextInNote(text) {
    // do the same as add note function
    AddNote(text)
    return "Updated"
}

// Delete
Any DeleteNote(text) {
    RemoveMapValue(Get("Notes"), text)
    return "Removed"
}




// // // Application global data getter/setter \\ \\ \\
Any Set(name, value) {
    SetMapValue(GetStatic("GlobalData"), name, value)
    return 0
}

Any Get(name) {
    return GetMapValue(GetStatic("GlobalData"), name)
}




// // // Platform Calls \\ \\ \\
Any AddGetEndpoint(name) {
    __platform_call("AddGetEndpoint", name, 1)
    return 0
}

Any AddPostEndpoint(name) {
    __platform_call("AddPostEndpoint", name, 1)
    return 0
}