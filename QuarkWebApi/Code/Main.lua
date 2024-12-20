import "../Libraries"

Number Main() {
    SetStatic("GlobalData", CreateMap())
    SetMapValue(GetStatic("GlobalData"), "Notes", CreateMap())

    AddGetEndpoint("PrintNotes")
    AddPostEndpoint("AddNote")

    return 0
}




// // Application request handlers // //
Any PrintNotes() {
    return SerializeMap(Get("Notes"))
}

Any AddNote(text) {
    map = DeserializeIntoMap(text)
    SetMapValue(Get("Notes"), GetMapValue(map, "name"), map)
    return "Added"
}




// // Application global data getter/setter // //
Any Set(name, value) {
    SetMapValue(GetStatic("GlobalData"), name, value)
    return 0
}

Any Get(name) {
    return GetMapValue(GetStatic("GlobalData"), name)
}




// // Platform Calls // //
Any AddGetEndpoint(name) {
    __platform_call("AddGetEndpoint", name, 1)
    return 0
}

Any AddPostEndpoint(name) {
    __platform_call("AddPostEndpoint", name, 1)
    return 0
}