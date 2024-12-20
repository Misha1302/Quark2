import "../Libraries"

Number Main() {
    SetStatic("GlobalData", CreateMap())
    SetMapValue(GetStatic("GlobalData"), "Notes", CreateMap())

    AddGetEndpoint("Say")
    AddPostEndpoint("AddNote")

    return 0
}

Any Say() {
    return Concat("Say: ", SerializeMap(Get("Notes")))
}

Any AddNote(text) {
    map = DeserializeIntoMap(text)
    SetMapValue(Get("Notes"), GetMapValue(map, "name"), map)
    return ""
}

Any Set(name, value) {
    SetMapValue(GetStatic("GlobalData"), name, value)
    return 0
}

Any Get(name) {
    return GetMapValue(GetStatic("GlobalData"), name)
}

Any AddGetEndpoint(name) {
    __platform_call("AddGetEndpoint", name, 1)
    return 0
}

Any AddPostEndpoint(name) {
    __platform_call("AddPostEndpoint", name, 1)
    return 0
}