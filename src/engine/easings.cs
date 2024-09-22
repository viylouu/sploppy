partial class sploppy {
    const float _c4_ = 2*pi/3f;
    static float easeoutelastic(float x) => x<=0?0:x>=1?1:max(pow(2,-10*x)*sin((x*10-.75f)*_c4_)+1,0);
}