class particle { 
    public Vector2 pos { get; set; }
    public Vector2 vel { get; set; }
    public float size { get; set; }
    public float spawntime { get; set; }
    public float lasttime { get; set; }
    public Color col { get; set; }
    public Color dcol { get; set; }
    public bool ignoretime { get; set; }
    public bool gas { get; set; }
}